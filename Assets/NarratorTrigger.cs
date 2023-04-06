using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorTrigger : MonoBehaviour
{
    public int VoiceLine;
    bool alreadyPlayed = false;
    private void OnTriggerEnter(Collider other)
    {
        if(alreadyPlayed == true) return;
        NarratorSystem.Instance.SayVoiceLine(NarratorSystem.Instance.voiceLines[VoiceLine]);
        alreadyPlayed = true;
    }
}
