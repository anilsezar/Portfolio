using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Interfaces;

public interface IImageOfTheDayRepository
{
    Task<ImageOfTheDay> GetLatestBackgroundImageDetailsAsync();
}