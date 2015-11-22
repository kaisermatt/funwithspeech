using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Speech.Recognition;
using FunWithSpeech.Model;

namespace FunWithSpeech.Services
{
    public class TranscriptionService
    {
        private readonly ConcurrentDictionary<long, List<Metadata>> _concurrentDictionary;
        
        public TranscriptionService(ConcurrentDictionary<long, List<Metadata>> concurrentDictionary)
        {
            _concurrentDictionary = concurrentDictionary;
        }

        public void Transcribe(MediaSegment segment)
        {
            SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
            engine.LoadGrammar(new DictationGrammar());
            engine.SetInputToWaveFile(segment.file.FullName);

            var result = engine.Recognize();

            var metaDatum = new Metadata();
            metaDatum.Start = result.Audio.AudioPosition.TotalMilliseconds + segment.offsetMs;
            metaDatum.End = metaDatum.Start + segment.durationMs;
            metaDatum.EngineMetadata = new SpeechResults
            { 
                Text = result.Text, 
                Confidence = result.Confidence
            };

            _concurrentDictionary.AddOrUpdate(segment.FileId, new List<Metadata> {metaDatum}, (x, y) =>
            {
                y.Add(metaDatum);
                return y;
            });
        }
    }
}