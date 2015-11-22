using System;

namespace FunWithSpeech.Model
{
    public class Request
    {
        public string filePath {get; set;}
        public long fileId {get; set;}
        public DateTime timestamp {get; set;}
    }
}