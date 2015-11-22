using System.IO;

namespace FunWithSpeech.Model
{
    public class MediaSegment
    {
        public long FileId { get; set; }
        public FileInfo File { get; set; }
        public double OffsetMs { get; set; }
        public double DurationMs { get; set; }
    }
}