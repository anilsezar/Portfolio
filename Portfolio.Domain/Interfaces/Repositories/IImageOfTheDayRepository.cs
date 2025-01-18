using Portfolio.Domain.Entities;

namespace Portfolio.Domain.Interfaces.Repositories;

public interface IImageOfTheDayRepository
{
    Task<ImageOfTheDay> GetLatestBackgroundImageDetailsAsync();
}