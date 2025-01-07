using Google.Protobuf.WellKnownTypes;
using Portfolio.Infrastructure.Repositories;

namespace Portfolio.Grpc.Services;

public class GetBackgroundImageService(ImageOfTheDayRepository repository) : BackgroundImages.BackgroundImagesBase
{
    public override async Task<BackgroundImageDetails> Get(Empty empty, ServerCallContext context)
    {
        var img = await repository.GetLatestBackgroundImageDetailsAsync();
        
        return new BackgroundImageDetails { Url = img.ImageUrl, Source = (ImageOfTheDaySource)img.Source, AltText = img.AltText};
    }
}
