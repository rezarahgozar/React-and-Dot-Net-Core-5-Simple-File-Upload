using FileUpload.Services.Interfaces;
using FileUpload.ViewModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace FileUpload.Services
{
    public class FileUploadService : IFileUploadService
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public FileUploadService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Delete(string name)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), @"images\", name);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(path);
                }
            }
            catch (Exception)
            {

                return "can not delete the file";
            }

            return name;
        }

        public List<FileInfoViewModel> List()
        {
            List<FileInfoViewModel> myList = new List<FileInfoViewModel>();
            var _baseURL = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";

            var path = Path.Combine(Directory.GetCurrentDirectory(), @"images\");
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] Files = dir.GetFiles().OrderByDescending(p => p.CreationTime).ToArray();

            foreach (var file in Files)
            {
                myList.Add(new FileInfoViewModel { 
                    Name = file.Name,
                    Length = (file.Length / 100).ToString() + " kb",
                    UploadDate = file.CreationTime.ToShortDateString(),
                    Extension = file.Extension, Photo = _baseURL +"images/"+ file.Name });
            }

            return myList;
        }

        public async Task<string> Upload(IFormFile file, string jsonElement)
        {
            var MyObj = JsonConvert.DeserializeObject(jsonElement);
            // var dataUser = jsonElement.GetProperty("user");

            if (file != null && file.Length > 0)
            {
                string[] extension = file.FileName.ToString().Split(".");

                var guid = Guid.NewGuid();

                if (extension[1] == "jpg" || extension[1] == "jpeg" || extension[1] == "png")
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "images", guid.ToString() + "." + extension[1]);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return file.FileName;
                }
                else
                {
                    return "format not support";
                }
            }

            return "";
        }
    }
}
