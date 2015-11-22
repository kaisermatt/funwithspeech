using System;

namespace FunWithSpeech.Model
{
    public class JobRequest
    {
        public string FilePath {get; set;}
        public long FileId {get; set;}
        public DateTime Timestamp {get; set;}
    }
}