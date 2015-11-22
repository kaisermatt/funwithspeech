using System;
using System.IO;
using System.Net;

namespace FunWithSpeech.Services
{
    public class DownloaderService
    {
        private readonly string _destinationPath;

        public DownloaderService(string destinationPath)
        {
            _destinationPath = destinationPath;
        }

        public FileInfo Download(string filePath, long fileId)
        {
            using (var client = new WebClient())
            {
                var destinationPath = Path.Combine(_destinationPath, fileId.ToString());
                
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                var destinationFileName = Path.Combine(_destinationPath, fileId.ToString(), "original.mp4");

                client.DownloadFile(new Uri(filePath), destinationFileName);
                return new FileInfo(destinationFileName);
            }
        }
        public void CleanUp(long fileId)
        {
            var cleanUpPath = Path.Combine(_destinationPath, fileId.ToString());
            if (Directory.Exists(cleanUpPath))
            {
                Directory.Delete(cleanUpPath, true);
            }
        }
  
    }
}