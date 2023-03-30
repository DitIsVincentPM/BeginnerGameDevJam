using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarratorSystem : MonoBehaviour
{
    [System.Serializable]
    public class VoiceLine
    {
        [SerializeField]
        public AudioClip audio;

        [SerializeField]
        public string text;

        public VoiceLine(AudioClip Audio, string Text)
        {
            audio = Audio;
            text = Text;
        }
    }

    [SerializeField]
    public List<VoiceLine> voiceLines;

    [Header("Settings")]
    [SerializeField]
    SoundSystem soundSystem;

    [SerializeField]
    TMP_Text subtitle;

    private float currentLength = 0;
    private float timer = 0;
    private bool sayingLine = false;

    void Update()
    {
        if (sayingLine == true)
        {
            timer += Time.deltaTime;
            if (timer > currentLength)
            {
                subtitle.text = "";
                timer = 0;
                currentLength = 0;
                sayingLine = false;
            }
        }
    }

    public void SayVoiceLine(VoiceLine voiceline)
    {
        soundSystem.PlayNarrator(voiceline.audio, 1);
        currentLength = voiceline.audio.length;
        subtitle.text = voiceline.text;
        sayingLine = true;
    }
}
