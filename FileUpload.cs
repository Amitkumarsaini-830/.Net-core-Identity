using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.aadviktech.IMS.Constant
{
    public static class FileUpload
    {
        private static IHostingEnvironment environment;
        public static async Task<string> UploadedFile(IFormFile model)
        {
            string uniqueFileName = null;

            string uploadsFolder = Path.Combine(environment.WebRootPath,"Transaction");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
        public static async Task<Dictionary<string, object>> DeleteFile(string file)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            {
                try
                {
                    string fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Profileimages/");
                    var fileList = Directory.EnumerateFiles(fileDirectory, "*", SearchOption.AllDirectories).Select(Path.GetFileName);
                    // string fileDirectory = fileDirectory;
                    string webRootPath = environment.WebRootPath;
                    var fileName = "";
                    fileName = file;
                    string fullPath = webRootPath + "/Profileimages/" + file;
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        dict.Add("Status", true);
                        dict.Add("Message", "File deleted succsessfull");
                    }
                }
                catch (Exception ex)
                {
                    dict.Add("Status", false);
                    dict.Add("Message", "An error occured during File deleting");

                }
            }
            return dict;
        }

    }
}
