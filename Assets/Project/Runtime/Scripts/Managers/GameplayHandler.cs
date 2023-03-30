using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayHandler : MonoBehaviour
{
    public static new GameplayHandler singleton { get; set; }

    [Header("Imports")]
    [SerializeField]
    private List<LichtFlasher> _lights;

    [SerializeField]
    NarratorSystem narratorSystem;

    [SerializeField]
    DoorController FirstDoor;

    [SerializeField]
    private AudioClip alarm;

    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        foreach (GameObject source in GameObject.FindGameObjectsWithTag("AlarmSource"))
        {
            _lights.Add(source.transform.GetComponentInChildren<LichtFlasher>());
        }
        DisarmAlarm();
        narratorSystem.SayVoiceLine(narratorSystem.voiceLines[0]);
    }

    public void DisarmAlarm()
    {
        foreach (LichtFlasher light in _lights)
        {
            light.endColor = new Color(1, 1, 1);
            light.startColor = new Color(1, 1, 1);
            light.transform.GetComponentInChildren<Light>().intensity = 12;
        }
        SoundSystem.singleton.StopAlarm();
    }

    public void ArmAlarm()
    {
        foreach (LichtFlasher light in _lights)
        {
            light.endColor = Color.red;
            light.startColor = new Color(0.78f, 0.1945087f, 0.1993889f);
            light.transform.GetComponentInChildren<Light>().intensity = 25;
        }
        SoundSystem.singleton.PlayAlarm(alarm, 0.1f);
    }

    public void CompleteHack(GameObject hackingObject)
    {
        switch (hackingObject.name)
        {
            case "Monitor":
                narratorSystem.SayVoiceLine(narratorSystem.voiceLines[1]);
                FirstDoor.activationState = DoorController.DoorActivation.Proximity;
                ArmAlarm();
                break;
        }
    }
}
