using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderProgress : MonoBehaviour
{
    [Header("Settings")]
    private float targetProgress = 0;

    [SerializeField]
    private float fillSpeed = 0.1f;
    public GameObject obj;

    [Header("Sound Clips")]
    [SerializeField]
    private AudioClip hackingSound;

    public void SetProgress(GameObject objects, int target) {
        obj = objects;
        targetProgress = target;
    }

    public void StartHacking(GameObject objects)
    {
        SetProgress(objects, 1);
        NotificationSystem.singleton.Notification(NotificationSystem.NotificationType.Hacking);
        SoundSystem.singleton.PlaySound(
            hackingSound,
            FindFirstObjectByType<PlayerController>().gameObject.transform.position
        );
    }

    public void StopHacking()
    {
        NotificationSystem.singleton.NotificationCallback(
            NotificationSystem.NotificationType.Hacking
        );
        GameplayHandler.singleton.CompleteHack(obj);
        UISystem.singleton.NotificationSlider.gameObject.SetActive(false);
    }

    public void DownloadingDisk()
    {
        NarratorSystem.singleton.SayVoiceLine(NarratorSystem.singleton.voiceLines[2]);
    }

    void Update()
    {
        if (targetProgress > 0)
        {
            if (UISystem.singleton.NotificationSlider.value <= targetProgress)
            {
                UISystem.singleton.NotificationSlider.value += fillSpeed * Time.deltaTime;
            }

            if (UISystem.singleton.NotificationSlider.value >= 1)
            {
                UISystem.singleton.NotificationSlider.value = 0;
                switch (obj.name)
                {
                    case "Monitor":
                        StopHacking();
                        break;
                    case "Server":
                        DownloadingDisk();
                        break;
                }
            }
        }
    }
}
