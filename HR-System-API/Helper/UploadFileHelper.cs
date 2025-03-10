﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace BLL.Helper
{
   public class UploadFileHelper
    {
        public static string SaveFile(IFormFile PhotoUrl, string FolderPath)
        {

            // Get Directory
            string FilePath = Directory.GetCurrentDirectory() + "/Files/" + FolderPath;

            // Get File Name
            string FileName = Guid.NewGuid() + Path.GetFileName(PhotoUrl.FileName);

            // Merge The Directory With File Name
            string FinalPath = Path.Combine(FilePath, FileName);

            // Save Your File As Stream "Data Overtime"
            using (var Stream = new FileStream(FinalPath, FileMode.Create))
            {
                PhotoUrl.CopyTo(Stream);
            }

            return FileName;
        }

        public static void RemoveFile(string FolderName, string RemovedFileName)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "/wwwroot/Files/" + FolderName + RemovedFileName))
            {
                File.Delete(Directory.GetCurrentDirectory() + "/wwwroot/Files/" + FolderName + RemovedFileName);
            }

        }

        internal static string SaveFile(object photoUrl, string v)
        {
            throw new NotImplementedException();
        }
    }
}
