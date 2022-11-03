using ETicaretAPI.Application.Services;
using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();

                return true;
            }
            catch (Exception ex)
            {
                //todo log çalışması!
                throw;
            }
        }

        private async Task<string> RenameFileAsync(string path, string fileName, bool first = true)
        {
            string newFileName = await Task.Run(async () =>
               {
                   string extension = Path.GetExtension(fileName);

                   string oldName = "";
                   string newFileName = string.Empty;

                   if (first)
                   {
                       oldName = Path.GetFileNameWithoutExtension(fileName);
                       newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
                   }
                   else
                   {
                       newFileName = fileName;
                       int indexNo1 = newFileName.IndexOf("-");
                       if (indexNo1 == -1)
                           newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                       else
                       {
                           int indexNo2 = newFileName.IndexOf(".");
                           string fileNo = newFileName.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);
                           int _fileNo = int.Parse(fileNo);
                           _fileNo++;
                           newFileName = newFileName.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                                                        .Insert(indexNo1 + 1, _fileNo.ToString());
                       }

                   }


                   if (File.Exists($@"{path}\{newFileName}"))
                       return await RenameFileAsync(path, newFileName, false);
                   else
                       return newFileName;

               });

            return newFileName;
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(
                _webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await RenameFileAsync(uploadPath, file.FileName);

                bool result = await CopyFileAsync($@"{uploadPath}\{fileNewName}", file);
                datas.Add((fileNewName, $@"{uploadPath}\{fileNewName}"));
                results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true)))
                return datas;

            return null;
            //todo yukaridaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığına dair uyarıcı bir exception oluşturulup fırlatılması gerekiyor

        }
    }
}
