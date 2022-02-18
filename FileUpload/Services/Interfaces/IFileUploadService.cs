using FileUpload.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Services.Interfaces
{
    public interface IFileUploadService
    {
        List<FileInfoViewModel> List();
        Task<string> Upload(IFormFile file, string jsonElement);
        string Delete(string name);
    }
}
