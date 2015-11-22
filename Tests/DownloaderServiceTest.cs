using System.IO;
using FunWithSpeech.Controllers;
using FunWithSpeech.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class DownloaderServiceTest
    {
        private const string WorkingDirectory = "c:\\test";

        [TestMethod]
        public void DoesDownload()
        {
            var sut = new DownloaderService(WorkingDirectory);
            var fileInfo = sut.Download(
                "https://s3.amazonaws.com/inspirent/tv-recordings/15/11/06/2377fa19-29e4-4216-59f1-71359dc046d9.mp4", 1);
            Assert.IsTrue(fileInfo.Exists);
        }
    

        [TestMethod]
        public void DoesChunk()
        {
            var sut = new ChunkerService(DefaultController.ChunkMs);
            var file = new FileInfo(Path.Combine(WorkingDirectory, "1" ,"original.mp4"));
            var fileInfos = sut.Chunk(file, 1);
            foreach (var fileInfo in fileInfos)
            {
                Assert.IsTrue(fileInfo.file.Exists);
            }
        }
    }

   
}
