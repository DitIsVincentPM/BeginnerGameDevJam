using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderProgress : MonoBehaviour
{
    private float targetProgress = 0;
    private float fillSpeed = 0.1f;
    private GameObject obj;
    
    [Header("Sound Clips")]
    [SerializeField]
    private AudioClip hackingSound;
    [SerializeField]
    private AudioClip uploadSound;

    // Set the progressbars target
    public void SetProgress(GameObject objects, int target)
    {
        obj = objects;
        targetProgress = target;
    }
    // Gets called when user Click's E on the Monitor
    public void StartHacking(GameObject objects)
    {
        SetProgress(objects, 1);
        Interact.Instance.SetInteractable("Monitor", false);
        NotificationSystem.Instance.Notification(NotificationSystem.NotificationType.Hacking);
        SoundSystem.Instance.PlaySound(
            hackingSound,
            FindFirstObjectByType<PlayerController>().gameObject.transform.position
        );
    }
    // Gets called on the callback when hacking of the Monitor is done
    public void StopHacking()
    {
        NotificationSystem.Instance.NotificationCallback(
            NotificationSystem.NotificationType.Hacking
        );
        GameplayHandler.Instance.CompleteHack(obj);
        UISystem.Instance.NotificationSlider.gameObject.SetActive(false);
    }
    // Gets called when you found the right server
    public void DownloadingDisk()
    {
        NotificationSystem.Instance.NotificationCallback(
            NotificationSystem.NotificationType.Download
        );
        NarratorSystem.Instance.SayVoiceLine(NarratorSystem.Instance.voiceLines[2]);
        GameplayHandler.Instance.CDReader.GetComponent<Outline>().enabled = true;
        GameObject.FindObjectOfType<PlayerController>().inInventory.Add("Disk");
        Interact.Instance.SetInteractable("CDReader", true);
        GameplayHandler.Instance.currentPuzzle = 2;
        targetProgress = 0;
    }
    // Gets called when Disk is uploaded to the disk reader
    public void UploadedDisk()
    {
        NarratorSystem.Instance.SayVoiceLine(NarratorSystem.Instance.voiceLines[3]);
        GameplayHandler.Instance.DisarmAlarm();
        NotificationSystem.Instance.NotificationCallback(NotificationSystem.NotificationType.Upload  );
        targetProgress = 0;
    }

    // This gets called when disk is uploading to the disk reader
    public void StartDiskUpload(GameObject objects) {
        SetProgress(objects, 1);
        Interact.Instance.SetInteractable("CDReader", false);
        GameplayHandler.Instance.CDReader.GetComponent<Outline>().enabled = false;
        NotificationSystem.Instance.Notification(NotificationSystem.NotificationType.Upload, "Disk Content");
        SoundSystem.Instance.PlaySound(
            uploadSound,
            objects.transform.position
        );
    }

    void Update()
    {
        if (targetProgress > 0)
        {
            if (UISystem.Instance.NotificationSlider.value <= targetProgress)
            {
                UISystem.Instance.NotificationSlider.value += fillSpeed * Time.deltaTime;
            }

            // This sends a callback when Progressbar is full
            if (UISystem.Instance.NotificationSlider.value >= 1)
            {
                UISystem.Instance.NotificationSlider.value = 0;
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
