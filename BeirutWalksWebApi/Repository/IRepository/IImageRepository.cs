using BeirutWalksDomains.Dto;
using BeirutWalksDomains.Models;

namespace BeirutWalksWebApi.Repository.IRepository
{
    public interface IImageRepository
    {
        Task<Image> UploadImage(Image imageUpload);
    }
}
