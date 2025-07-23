using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using System.Collections.Generic;
using Meta.WitAi.TTS.Integrations;

[RequireComponent(typeof(TTSSpeaker))]
[RequireComponent(typeof(TTSSpeechSplitter))]
public class TTSManager : MonoBehaviour
{
    public static TTSManager Instance { get; private set; }
    [SerializeField] private TTSSpeaker speaker;
    [SerializeField] private TTSSpeechSplitter splitter;
    void Awake()
    {
        speaker = GetComponent<TTSSpeaker>();
        splitter = GetComponent<TTSSpeechSplitter>();
        speaker.Pause();
    }

    public void Speak(string speech)
    {
        speaker.Stop();
        if (speech.Length > 260)
        {
            List<string> phrases = new List<string> { speech };

            splitter.OnPreprocessTTS(speaker, phrases);

            foreach (var phrase in phrases)
            {
                speaker.SpeakQueued(phrase);
            }
        }
        else
        {
            speaker.Speak(speech);
        }
    }

    public void StopAudio()
    {
        speaker.Stop();
    }
}
