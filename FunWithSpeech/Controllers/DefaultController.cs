using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using FunWithSpeech.Model;
using FunWithSpeech.Services;

namespace FunWithSpeech.Controllers
{
    public class DefaultController : ApiController
    {
        private readonly DownloaderService _downloader;
        private readonly ChunkerService _chunker;
        private readonly TranscriptionService _transcriber;

        //This should really be a repo
        private static readonly ConcurrentDictionary<long, List<Metadata>> ConcurrentDictionary = new ConcurrentDictionary<long, List<Metadata>>();

        //These should be configs
        public const int ChunkMs = 10000;
        private string path = "c:\\WorkingDirectory";

        public DefaultController()
        {
            //These should be injected.
            _downloader = new DownloaderService(path);
            _chunker = new ChunkerService(ChunkMs);
            _transcriber =  new TranscriptionService(ConcurrentDictionary);
        }

        public IHttpActionResult Get(long id)
        {
            return Ok(new JobResult
            {
                FileId = id,
                Series = ConcurrentDictionary[id]
            });
        }

        // POST: api/Default
        public IHttpActionResult Post([FromBody]JobRequest value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            ConcurrentDictionary.AddOrUpdate(value.FileId, new List<Metadata>(), (x, y) => y);

            Task.Run(() =>
            {
                var file = _downloader.Download(value.FilePath, value.FileId);
                var chunks = _chunker.Chunk(file, value.FileId);
                
                foreach (var chunk in chunks)
                {
                    _transcriber.Transcribe(chunk);
                }
            });
            
            return Ok();
        }
    }
}
