using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Demo.PL.Helpers
{
    public static class DocumentSettings // we dont need to create opjects from this class so we make it static
    {
        public static string UploadFile(IFormFile file , string FolderName)
        {
            // 1- Get Located Folder path

            string FolderPath=Path.Combine(Directory.GetCurrentDirectory() , "WWWroot\\Files" , FolderName);

            //2- Get file name and make it unique

            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            //3- Get file path(folder path + filename)
            string FilePath=Path.Combine(FolderPath,FileName);

            //4- Save file as streams ==> Data per time
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);

            //5- return file name
            return FileName;

        }

        public static void DeleteFile(string FolderName,string Filename) {

            string FilePath=Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, Filename);
            if(File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
