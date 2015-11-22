using Newtonsoft.Json;

namespace FunWithSpeech.Model
{
    public class Metadata
    {
        [JsonProperty(PropertyName = "start")]
        public double Start {get; set;}
        [JsonProperty(PropertyName = "end")]
        public double End { get; set;}
        [JsonProperty(PropertyName = "engine-metadata")]
        public SpeechResults EngineMetadata {get; set;}
    }
}