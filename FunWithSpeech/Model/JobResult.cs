using System.Collections.Generic;
using Newtonsoft.Json;

namespace FunWithSpeech.Model
{
    public class JobResult
    {
        [JsonProperty(PropertyName = "fileId")]
        public long FileId {get; set;}
        [JsonProperty(PropertyName = "series")]
        public IEnumerable<Metadata> Series { get; set; }
    }
}