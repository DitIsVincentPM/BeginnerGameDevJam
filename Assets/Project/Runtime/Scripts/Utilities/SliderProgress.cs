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
    [SerializeField]
    private AudioClip uploadSound;

    public void SetProgress(GameObject objects, int target)
    {
        obj = objects;
        targetProgress = target;
    }

    public void StartHacking(GameObject objects)
    {
        SetProgress(objects, 1);
        Interact.singleton.SetInteractable("Monitor", false);
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
        NotificationSystem.singleton.NotificationCallback(
            NotificationSystem.NotificationType.Download
        );
        NarratorSystem.singleton.SayVoiceLine(NarratorSystem.singleton.voiceLines[2]);
        GameplayHandler.singleton.CDReader.GetComponent<Outline>().enabled = true;
        GameObject.FindObjectOfType<PlayerController>().inInventory.Add("Disk");
        Interact.singleton.SetInteractable("CDReader", true);
        GameplayHandler.singleton.currentPuzzle = 2;
        targetProgress = 0;
    }

    public void UploadedDisk()
    {
        NarratorSystem.singleton.SayVoiceLine(NarratorSystem.singleton.voiceLines[3]);
        GameplayHandler.singleton.DisarmAlarm();
        NotificationSystem.singleton.NotificationCallback(NotificationSystem.NotificationType.Upload);
        targetProgress = 0;
    }

    public void StartDiskUpload(GameObject objects) {
        SetProgress(objects, 1);
        Interact.singleton.SetInteractable("CDReader", false);
        GameplayHandler.singleton.CDReader.GetComponent<Outline>().enabled = false;
        NotificationSystem.singleton.Notification(NotificationSystem.NotificationType.Upload, "Disk Content");
        SoundSystem.singleton.PlaySound(
            uploadSound,
            objects.transform.position
        );
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
                    case "CDReader": 
                        UploadedDisk();
                        break;
                }
            }
        }
    }
}
