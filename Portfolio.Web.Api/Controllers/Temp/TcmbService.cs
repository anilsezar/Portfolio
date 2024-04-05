using System.Xml.Linq;
using Portfolio.Domain.Entities.WebAppEntities;

namespace Portfolio.Web.Api.Controllers.Temp;

public class TcmbService
{
    private readonly WebAppDbContext _dbContext;

    public TcmbService(WebAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private const string DateCodeTR = "Tarih";
    private const string DateCodeEN = "Date";
    private const string CurrencyCode = "Kod";
    private const string Unit = "Unit";
    private const string BuyPrice = "ForexBuying";
    private const string SellPrice = "ForexSelling";

    private const string XmlUrl = "https://www.tcmb.gov.tr/kurlar/today.xml";

    public GenericResponse GetCurrenciesAndAddThemToDb()
    {
        var rates = MapTheXml(GetXmlFromUrlAsync().Result);

        var neededCurrencies = GetNeededCurrencies();

        rates.RemoveAll(x => !neededCurrencies.Contains(x.Currency));

        VerifyTheResults(rates, neededCurrencies);

        rates = AddNumericCodesToCurrencies(rates);
        
        _dbContext.

        _session.CoreAddIdentity(rates);

        return new GenericResponse(true, "");
    }

    /*
        **************************************************************
        CEVAP ÖRNEĞİ 
        (url ziyaret edilerek tam örneğe erişilebilir):
        **************************************************************
        <Currency CrossOrder = "1" Kod= "AUD" CurrencyCode= "AUD" >
                < Unit > 1 </ Unit >
                < Isim > AVUSTRALYA DOLARI</Isim>
                <CurrencyName>AUSTRALIAN DOLLAR</CurrencyName>
                <ForexBuying>17.6905</ForexBuying>
                <ForexSelling>17.8058</ForexSelling>
                <BanknoteBuying>17.6091</BanknoteBuying>
                <BanknoteSelling>17.9127</BanknoteSelling>
                    <CrossRateUSD>1.5610</CrossRateUSD>
                    <CrossRateOther/>
        </Currency>
    */

    private async Task<XDocument> GetXmlFromUrlAsync()
    {
        using HttpClient client = new HttpClient();
        var response = await client.GetStringAsync(XmlUrl);
        return XDocument.Parse(response);
    }

    private List<ExchangeRate> MapTheXml(XDocument xmlDoc)
    {
        var list = new List<ExchangeRate>();

        var tcmbDate = ExtractDateFromXml(xmlDoc);

        foreach (var currency in xmlDoc.Descendants("Currency"))
        {
            var parsedVal = currency.Element(Unit)?.Value ?? "";
            var currencyUnit = int.Parse(parsedVal.IsNullOrEmpty() ? "-1" : parsedVal);
            var currencyName = currency.Attribute(CurrencyCode)?.Value;

            if (currencyUnit == -1 || currencyName.IsNullOrEmpty())
                continue;

            parsedVal = currency.Element(BuyPrice)?.Value ?? "";
            var buyPrice = decimal.Parse(parsedVal.IsNullOrEmpty() ? "-1" : parsedVal, CultureInfo.InvariantCulture);
            parsedVal = currency.Element(SellPrice)?.Value ?? "";
            var sellPrice = decimal.Parse(parsedVal.IsNullOrEmpty() ? "-1" : parsedVal, CultureInfo.InvariantCulture);

            list.Add(new ExchangeRate
            {
                TcmbReportDate = tcmbDate,
                Currency = currencyName,
                Unit = currencyUnit,
                ForexBuying = buyPrice,
                ForexSelling = sellPrice
            });
        }

        return list;
    }
    private DateTime ExtractDateFromXml(XDocument xmlDoc)
    {
        if (xmlDoc.Root?.Attribute(DateCodeTR)?.Value != null)
            return DateTime.ParseExact(xmlDoc.Root.Attribute(DateCodeTR)?.Value, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        else if (xmlDoc.Root?.Attribute(DateCodeEN)?.Value != null)
            return DateTime.ParseExact(xmlDoc.Root.Attribute(DateCodeEN)?.Value, "dd/MM/yyyy", CultureInfo.InvariantCulture);

        var errMsg = "TCMB'den döviz bilgilerini alamadık. Bu durum çok yeri bozar. Exception fırlatacağız";
        _logger.LogCritical(errMsg);
        throw new Exception(errMsg);
    }

    private List<string> GetNeededCurrencies()
    {
        var neededCurrencies = new List<string>(Enum.GetNames(typeof(CurrencyCodeEnum)));
        neededCurrencies.Remove(CurrencyCodeEnum.TRY.ToString());
        return neededCurrencies;
    }

    private void VerifyTheResults(List<ExchangeRate> rates, List<string> neededCurrencies)
    {
        var errMsg = "";
        foreach (var code in neededCurrencies)
        {
            var rate = rates.SingleOrDefault(x => x.Currency == code);

            if (rate != null && rate.ForexBuying != -1 && rate.ForexBuying != 0 && rate.ForexSelling != -1 && rate.ForexSelling != 0)
                continue;

            if (errMsg != "")
                errMsg += $", {code}";
            else
                errMsg += code;
        }

        if (errMsg != "")
        {
            var temp = errMsg;
            errMsg = $"{XmlUrl}'e yapılıp objeye maplenen işlemin sonucunda {temp} değer(leri) yok veya eksik. Tcmb'den değerler alınamamış olabilir mi? Rate'ler: {JsonConvert.SerializeObject(rates)}";

            _logger.LogCritical(errMsg);
            throw new Exception(errMsg);
        }
    }

    private List<ExchangeRate> AddNumericCodesToCurrencies(List<ExchangeRate> l)
    {
        foreach (var i in l)
        {
            if (!Enum.IsDefined(typeof(CurrencyCodeEnum), i.Currency))
            {
                var errMsg = $"{nameof(CurrencyCodeEnum)}'da beklenmeyen bir değer arandı. Aranan değer: {i.Currency}";
                _logger.LogCritical(errMsg);
                throw new Exception(errMsg);
            }

            var foundCurrency = (CurrencyCodeEnum)Enum.Parse(typeof(CurrencyCodeEnum), i.Currency);
            i.CurrencyNumericCode = (short)foundCurrency;
        }

        return l;
    }
}