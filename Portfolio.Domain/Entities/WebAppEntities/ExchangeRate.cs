using System.ComponentModel.DataAnnotations;
using Portfolio.Domain.Constants;

namespace Portfolio.Domain.Entities.WebAppEntities;

/// <summary>
/// TCMB'den aldığımız döviz alım/satım değerleri
/// </summary>
[Comment("Tcmb ")]

public class ExchangeRate
{
    /// <summary>
    /// Tcmb bu raporu ne zaman oluşturduğunu iletiyor? Mesela ayın 11'inde saat 11:30'da çektiğimiz raporun tarihi düne ait.
    /// </summary>
    [Required(ErrorMessage = Etcetera.DataAnnotation_RequiredErrorMsg)]
    public DateTime TcmbReportDate { get; set; }
    
    [Required(ErrorMessage = Etcetera.DataAnnotation_RequiredErrorMsg)]
    public string Currency { get; set; }

    [Required(ErrorMessage = Etcetera.DataAnnotation_RequiredErrorMsg)]
    public short CurrencyNumericCode { get; set; }
    
    [Required(ErrorMessage = Etcetera.DataAnnotation_RequiredErrorMsg)]
    public int Unit { get; set; }
    
    [Required(ErrorMessage = Etcetera.DataAnnotation_RequiredErrorMsg)]
    public decimal ForexBuying { get; set; }
    
    [Required(ErrorMessage = Etcetera.DataAnnotation_RequiredErrorMsg)]
    public decimal ForexSelling { get; set; }
}