using BeirutWalksDomains.Models;
using BeirutWalksWebApi.Data;
using BeirutWalksWebApi.Repository.IRepository;

namespace BeirutWalksWebApi.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHost;
        private readonly IHttpContextAccessor httpContext;

        public ImageRepository(ApplicationDbContext db, IWebHostEnvironment webHost,IHttpContextAccessor httpContext)
        {
            this.db = db;
            this.webHost = webHost;
            this.httpContext = httpContext;
        }
        public async Task<Image> UploadImage(Image imageUpload)
        {
            string folderpath = Path.Combine(webHost.ContentRootPath, "Images", $"{imageUpload.FileName}{imageUpload.FileExtension}");
            using Stream stream = new FileStream(folderpath, FileMode.Create) ;
            await imageUpload.File.CopyToAsync(stream);
            string urlFilePath = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}{httpContext.HttpContext.Request.PathBase}/Images/{imageUpload.FileName}{imageUpload.FileExtension}";

            imageUpload.FilePath= urlFilePath;
            await db.Images.AddAsync(imageUpload);
            await db.SaveChangesAsync();
            return imageUpload;
        }
    }
}
