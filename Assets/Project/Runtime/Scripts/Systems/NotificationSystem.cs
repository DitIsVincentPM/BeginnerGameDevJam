using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificationSystem : StaticInstance<NotificationSystem>
{
    public enum NotificationType
    {
        Hacking,
        Upload,
        Download,
        Error,
        Hack,
        Warning,
        Complete,
    }

    [Header("Notification Types")]
    [SerializeField]
    public GameObject uploadDownload;

    [SerializeField]
    Animator animator;
    AnimationClip notificationDone;

    float timer;
    float waitTime;

    void Update()
    {
        if (waitTime > 0)
        {
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                NotificationCallback(NotificationType.Error);
                timer = 0;
                waitTime = 0;
            }
        }
    }

    public void Notification(NotificationType type, string value = null)
    {
        switch (type)
        {
            case NotificationType.Hacking:
                uploadDownload.GetComponentInChildren<TMP_Text>().text =
                    value != null ? "Hacking " + value : "Hacking";
                uploadDownload.transform.GetChild(0).gameObject.SetActive(true);
                uploadDownload.GetComponent<Image>().color = new Color(0, 0, 0, 0.4039216f);
                animator.SetFloat("notification", 1);
                break;
            case NotificationType.Download:
                uploadDownload.GetComponentInChildren<TMP_Text>().text =
                    value != null ? "Downloading " + value : "Downloading";
                uploadDownload.transform.GetChild(0).gameObject.SetActive(true);
                uploadDownload.GetComponent<Image>().color = new Color(0, 0, 0, 0.4039216f);
                animator.SetFloat("notification", 1);
                break;
            case NotificationType.Upload:
                uploadDownload.GetComponentInChildren<TMP_Text>().text =
                    value != null ? "Uploading " + value : "Uploading";
                uploadDownload.transform.GetChild(0).gameObject.SetActive(true);
                uploadDownload.GetComponent<Image>().color = new Color(0, 0, 0, 0.4039216f);
                animator.SetFloat("notification", 1);
                break;
            case NotificationType.Error:
                uploadDownload.GetComponentInChildren<TMP_Text>().text =
                    value != null ? "\r\nError " + value : "Error";
                uploadDownload.transform.GetChild(0).gameObject.SetActive(false);
                uploadDownload.GetComponent<Image>().color = new Color(
                    0.8773585f,
                    0,
                    0,
                    0.4039216f
                );
                animator.SetFloat("animSpeed", 5);
                animator.SetFloat("notification", 1);
                waitTime = 0.2f;
                break;
            case NotificationType.Hack:
                uploadDownload.GetComponentInChildren<TMP_Text>().text =
                    value != null ? "\r\nHacked " + value : "\r\nFailed Hack";
                uploadDownload.transform.GetChild(0).gameObject.SetActive(false);
                if (value == null)
                {
                    uploadDownload.GetComponent<Image>().color = new Color(
                        0.8773585f,
                        0,
                        0,
                        0.4039216f
                    );
                }
                else
                {
                    uploadDownload.GetComponent<Image>().color = new Color(
                        0.002803495f,
                        0.5943396f,
                        0.182557f,
                        0.4039216f
                    );
                }
                animator.SetFloat("animSpeed", 5);
                animator.SetFloat("notification", 1);
                waitTime = 0.2f;
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
            case NotificationType.Hacking:
                animator.SetFloat("notification", 0);
                break;
            case NotificationType.Download:
                animator.SetFloat("notification", 0);
                break;
            case NotificationType.Upload:
                animator.SetFloat("notification", 0);
                break;
            case NotificationType.Error:
                animator.SetFloat("notification", 0);
                animator.SetFloat("animSpeed", 1);
                break;
        }
    }
}
