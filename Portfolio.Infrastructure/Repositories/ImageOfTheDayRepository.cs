using Microsoft.EntityFrameworkCore;
using Portfolio.Infrastructure.Constants;
using Portfolio.Domain.Entities;
using Portfolio.Domain.Enums;
using Portfolio.Domain.Interfaces;
using Serilog;

namespace Portfolio.Infrastructure.Repositories;

public class ImageOfTheDayRepository(PortfolioDbContext dbContext) : IImageOfTheDayRepository
{
    public async Task AddAsync(ImageOfTheDay image)
    {
        await dbContext.ImageOfTheDay.AddAsync(image);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<ImageOfTheDay> GetLatestAsync()
    {
        return await dbContext.ImageOfTheDay
            .Where(x => x.UrlWorks && x.DoIPreferToDisplayThis)
            .OrderByDescending(x => x.CreationTime)
            .FirstAsync();
    }
    
    public async Task<ImageOfTheDay> GetLatestBackgroundImageDetailsAsync()
    {
        var img = await dbContext.ImageOfTheDay
            .Where(x => x.UrlWorks && x.DoIPreferToDisplayThis)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (img != null)
        {
            Log.Information("Serving this background image: {ImageUrl}", img.ImageUrl);
            return img;
        }

        Log.Error("Cannot get a background image from the database. Default image served");
        return new ImageOfTheDay
        {
            ImageUrl = DefaultValues.DefaultBackgroundImage,
            AltText = DefaultValues.DefaultAltText,
            Source = ImageOfTheDaySource.Bing,
            UrlWorks = true,
            DoIPreferToDisplayThis = true
        };
    }
}