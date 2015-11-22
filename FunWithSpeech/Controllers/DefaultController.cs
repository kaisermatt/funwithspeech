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
        private readonly SpeechService _speaker;

        //This should really be a repo
        private static readonly ConcurrentDictionary<long, List<MetaData>> ConcurrentDictionary = new ConcurrentDictionary<long, List<MetaData>>();

        //These should be configs
        public const int ChunkMs = 10000;
        private string path = "c:\\WorkingDirectory";

        public DefaultController()
        {
            //These should be injected.
            _downloader = new DownloaderService(path);
            _chunker = new ChunkerService(ChunkMs);
            _speaker =  new SpeechService(ConcurrentDictionary);
        }

        public IHttpActionResult Get(long id)
        {
            return Ok(new Output
            {
                FileId = id,
                Series = ConcurrentDictionary[id]
            });
        }

        // POST: api/Default
        public IHttpActionResult Post([FromBody]Request value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var file = _downloader.Download(value.filePath, value.fileId);
            var chunks = _chunker.Chunk(file, value.fileId);

            ConcurrentDictionary.AddOrUpdate(value.fileId, new List<MetaData>(), (x, y) => y);

            Task.Run(() =>
            {
                foreach (var chunk in chunks)
                {
                    _speaker.Recognize(chunk);
                }
            });
            
            return Ok();
        }
    }
}
