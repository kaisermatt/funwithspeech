using System;
using System.Collections.Generic;
using System.IO;
using FunWithSpeech.Model;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace FunWithSpeech.Services
{
    public class SegmenterService
    {
        private readonly int _segmentLengthMs;

        public SegmenterService(int segmentLengthMs)
        {
            _segmentLengthMs = segmentLengthMs;
        }

        public IEnumerable<MediaSegment> Segment(FileInfo file, long fileId)
        {
            var source = new MediaFile(file.FullName);
            using (var engine = new Engine())
            {
                engine.GetMetadata(source);
                var progressMs = 0.0;
                while (progressMs < source.Metadata.Duration.TotalMilliseconds)
                {
                    var options = new ConversionOptions();
                    var endMs = Math.Min(progressMs + _segmentLengthMs, source.Metadata.Duration.TotalMilliseconds);
                    
                    options.CutMedia(TimeSpan.FromMilliseconds(progressMs),
                        TimeSpan.FromMilliseconds(endMs));

                    var outputFile = Path.Combine(file.DirectoryName,
                        string.Format("{0}_audio_{1}ms.wav", file.Name, progressMs));

                    engine.Convert(source, new MediaFile(outputFile), options);
                    yield return new MediaSegment
                    {
                        FileId = fileId,
                        File = new FileInfo(outputFile),
                        OffsetMs = progressMs,
                        DurationMs = endMs - progressMs
                    };
                    progressMs = endMs;
                }
            }
        }
    }
}