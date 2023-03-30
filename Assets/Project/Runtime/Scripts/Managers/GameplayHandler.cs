using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayHandler : MonoBehaviour
{
    public static new GameplayHandler singleton { get; set; }

    [Header("Imports")]
    NarratorSystem narratorSystem;

    void Awake() {
        singleton = this;
    }

    void Start() {
        narratorSystem.SayVoiceLine(narratorSystem.voiceLines[0]);
    }

    public void CompleteHack(GameObject hackingObject) {
        switch (hackingObject.name)
        {
            case "Monitor":
                narratorSystem.SayVoiceLine(narratorSystem.voiceLines[1]);
                break;
        }
    }
}
