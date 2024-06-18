using Api.Repositories.Interfaces;
using Models.Entities;

namespace Api.Repositories.Implementations;

public class ImageRepository : IImageRepository
{
    public Task<List<Image>> GetAllImages()
    {
        throw new NotImplementedException();
    }

    public Task<Image?> GetImageById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task AddImage(Image image)
    {
        throw new NotImplementedException();
    }

    public Task UpdateImage(Image image)
    {
        throw new NotImplementedException();
    }

    public Task DeleteImage(Guid id)
    {
        throw new NotImplementedException();
    }
}