using ETicaretAPI.Infrastructure.Operations;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService
    {
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
                       int indexNo1 = newFileName.LastIndexOf("-");
                       if (indexNo1 == -1)
                           newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                       else
                       {
                          
                           int indexNo2 = newFileName.IndexOf(".");
                           string fileNo = newFileName.Substring(indexNo1 + 1, indexNo2 - indexNo1 - 1);
                           if (int.TryParse(fileNo, out int _fileNo))
                           {
                               _fileNo++;
                               newFileName = newFileName.Remove(indexNo1 + 1, indexNo2 - indexNo1 - 1)
                                                            .Insert(indexNo1 + 1, _fileNo.ToString());
                           }
                           else newFileName = $"{Path.GetFileNameWithoutExtension(newFileName)}-2{extension}";
                       }

                   }


                   if (File.Exists($@"{path}\{newFileName}"))
                       return await RenameFileAsync(path, newFileName, false);
                   else
                       return newFileName;

               });

            return newFileName;
        }

    }
}
