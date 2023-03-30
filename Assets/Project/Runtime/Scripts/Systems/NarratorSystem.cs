using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarratorSystem : MonoBehaviour
{
    [Header("Voice Lines")]
    [SerializeField]
    AudioClip firstVoiceline;

    [SerializeField]
    string firstVoicelineText;

    [Header("Settings")]
    [SerializeField]
    SoundSystem soundSystem;
    [SerializeField] 
    TMP_Text subtitle;

    private float currentLength = 0;
    private float timer = 0;
    private bool sayingLine = false;

    // Start is called before the first frame update
    void Start()
    {
        VoiceLine(firstVoiceline, firstVoicelineText);
    }

    void Update()
    {
        if (sayingLine == true)
        {
            timer += Time.deltaTime;
            if (timer > currentLength) { 
                subtitle.text = "";
                timer = 0;
                currentLength = 0;
                sayingLine = false;
            }
        }
    }

    public void VoiceLine(AudioClip voiceline, string voicelineString)
    {
        soundSystem.PlayNarrator(voiceline, 1);
        currentLength = voiceline.length;
        subtitle.text = voicelineString;
        sayingLine = true;
    }
}
