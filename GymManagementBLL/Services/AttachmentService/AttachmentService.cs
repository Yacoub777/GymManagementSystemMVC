using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
//using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

namespace GymManagementBLL.Services.AttachmentService
{

    public class AttachmentService : IAttachmentService
    {
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly long maxFileSize = 5 * 1024 * 1024;
        private readonly IWebHostEnvironment webHost;

        public AttachmentService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }
        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (folderName is null || file is null || file.Length == 0) return null;
                if (file.Length > maxFileSize) return null;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension)) return null;


                var folderPath = Path.Combine(webHost.WebRootPath, "images", folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var fileName = Guid.NewGuid().ToString() + extension;

                var filePath = Path.Combine(folderPath, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

                return fileName;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed To Upload File To Folder = {folderName} : {ex}");
                return null;
            }


        }
        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;


                var fullPath = Path.Combine(webHost.WebRootPath,"images" , folderName , fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed To Delete File With Name = {folderName} : {ex}");
                return false;
            }
        }
    }
}
