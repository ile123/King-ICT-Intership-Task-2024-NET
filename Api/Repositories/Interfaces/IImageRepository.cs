using Models.Entities;

namespace Api.Repositories.Interfaces;

public interface IImageRepository
{
   Task<List<Image>> GetAllImages();
   Task<Image?> GetImageById(Guid id);
   Task AddImage(Image image);
   Task UpdateImage(Image image);
   Task DeleteImage(Guid id);
}