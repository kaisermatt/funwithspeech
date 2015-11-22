using System;
using System.Collections.Generic;
using System.IO;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace FunWithSpeech.Services
{
    public class ChunkerService
    {
        private readonly int _chunkMs;

        public ChunkerService(int chunkMs)
        {
            _chunkMs = chunkMs;
        }

        public IEnumerable<MediaSegment> Chunk(FileInfo file, long fileId)
        {
            var source = new MediaFile(file.FullName);
            using (var engine = new Engine())
            {
                engine.GetMetadata(source);
                var progressMs = 0.0;
                while (progressMs < source.Metadata.Duration.TotalMilliseconds)
                {
                    var options = new ConversionOptions();
                    var endMs = Math.Min(progressMs + _chunkMs, source.Metadata.Duration.TotalMilliseconds);
                    
                    options.CutMedia(TimeSpan.FromMilliseconds(progressMs),
                        TimeSpan.FromMilliseconds(endMs));

                    var outputFile = Path.Combine(file.DirectoryName,
                        string.Format("_audio{0}{1}.wav", file.Name, progressMs));

                    engine.Convert(source, new MediaFile(outputFile), options);
                    yield return new MediaSegment
                    {
                        FileId = fileId,
                        file = new FileInfo(outputFile),
                        offsetMs = progressMs,
                        durationMs = endMs - progressMs
                    };
                    progressMs = endMs;
                }
            }
        }
    }

    public class MediaSegment
    {
        public long FileId { get; set; }
        public FileInfo file { get; set; }
        public double offsetMs { get; set; }
        public double durationMs { get; set; }
    }
}