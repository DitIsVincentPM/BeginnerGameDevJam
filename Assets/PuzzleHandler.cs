using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : StaticInstance<PuzzleHandler>
{
    public List<GameObject> Screens;
    public List<float> ScreenValues;
    bool ButtonPressed;
    int rnd;
    public DoorController door;

    public bool HasKey;

    void Start() {
        rnd = Random.Range(0,5);
    }

    public void ButtonPress()
    {
        if(ButtonPressed == true) return;
        for (int i = 0; i < ScreenValues.Count; i++)
        {
            ScreenValues[i] += 1;
        }

        foreach (GameObject screen in Screens)
        {
            foreach (Material mat in screen.gameObject.GetComponent<Renderer>().materials)
            {
                if (mat.name == "Screen")
                {
                    mat.color = new Color(0.5849056f, 0.2071059f, 0.2014062f);
                }
            }
        }
        ButtonPressed = true;
    }

    public void Hacked(int i)
    {
        if(i == rnd) {
            NotificationSystem.Instance.Notification(NotificationSystem.NotificationType.Hack, "Key");
            HasKey = true;
            door.activationState = DoorController.DoorActivation.StayOpen;
            NarratorSystem.Instance.SayVoiceLine(NarratorSystem.Instance.voiceLines[7]);
        } else {
            NotificationSystem.Instance.Notification(NotificationSystem.NotificationType.Hack);
        }
        ScreenValues[i] = 2;
        foreach (Material mat in Screens[i].gameObject.GetComponent<Renderer>().materials)
        {
            if (mat.name == "Screen")
            {
                mat.color = new Color(0.5849056f, 0.2071059f, 0.2014062f);
            }
        }
    }
}
