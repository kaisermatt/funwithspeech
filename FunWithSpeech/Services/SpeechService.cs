using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Speech.Recognition;
using FunWithSpeech.Model;

namespace FunWithSpeech.Services
{
    public class SpeechService
    {
        private readonly ConcurrentDictionary<long, List<MetaData>> _concurrentDictionary;
        
        public SpeechService(ConcurrentDictionary<long, List<MetaData>> concurrentDictionary)
        {
            _concurrentDictionary = concurrentDictionary;
        }

        public void Recognize(MediaSegment segment)
        {
            SpeechRecognitionEngine engine = new SpeechRecognitionEngine();
            engine.LoadGrammar(new DictationGrammar());
            engine.SetInputToWaveFile(segment.file.FullName);

            var result = engine.Recognize();

            GetValue(result, segment);
        }
        
        private void GetValue(RecognitionResult result, MediaSegment segment)
        {
            var metaDatum = new MetaData();
            metaDatum.Start = result.Audio.AudioPosition.TotalMilliseconds + segment.offsetMs;
            metaDatum.End = metaDatum.Start + segment.durationMs;
            metaDatum.EngineMetadata = new Results
            { 
                Text = result.Text, 
               Confidence = result.Confidence
            };

            _concurrentDictionary.AddOrUpdate(segment.FileId, new List<MetaData> {metaDatum}, (x, y) =>
            {
                y.Add(metaDatum);
                return y;
            });
        }
    }
}