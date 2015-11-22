using System.Collections.Generic;
using Newtonsoft.Json;

namespace FunWithSpeech.Model
{
    public class Output
    {
        [JsonProperty(PropertyName = "fileId")]
        public long FileId {get; set;}
        public IEnumerable<MetaData> Series { get; set; }
    }
}