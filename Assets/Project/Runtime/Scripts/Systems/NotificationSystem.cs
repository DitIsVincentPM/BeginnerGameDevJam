using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotificationSystem : MonoBehaviour
{
    public static new NotificationSystem singleton { get; set; }

    public enum NotificationType
    {
        Upload,
        Download,
        FileNotFound,
        Error,
        Warning,
        Complete,
    }

    [Header("Notification Types")]
    [SerializeField]
    GameObject uploadDownload;

    [SerializeField]
    Animator animation;
    AnimationClip notificationDone;

    void Awake() {
        singleton = this;
    }

    public void Notification(NotificationType type)
    {
        switch (type)
        {
            case NotificationType.Download:
                uploadDownload.GetComponentInChildren<TMP_Text>().text = "Downloading";
                animation.SetFloat("notification", 1);
                Debug.Log("Playing Animation");
                break;
            case NotificationType.Upload:
                uploadDownload.GetComponentInChildren<TMP_Text>().text = "Uploading";
                animation.SetFloat("notification", 1);
                break;
            case NotificationType.FileNotFound:
                break;
            case NotificationType.Error:
                break;
            case NotificationType.Complete:
                break;
            case NotificationType.Warning:
                break;
        }
    }

    public void NotificationCallback(NotificationType type)
    {
        switch (type)
        {
            case NotificationType.Download:
                animation.SetFloat("notification", 0);
                break;
            case NotificationType.Upload:
                animation.SetFloat("notification", 0);
                break;
        }
    }
}
