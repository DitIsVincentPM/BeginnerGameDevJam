using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackingProgress : MonoBehaviour
{
    public Slider slider;
    public GameObject WorldCanvas;
    public bool enableSlider = false;

    private float targetProgress = 0;

    [SerializeField]
    private float fillSpeed = 0.2f;
    public GameObject hackingObject;
    bool sendNotification;

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
        slider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (enableSlider == true)
        {
            if(sendNotification == false) {
                NotificationSystem.singleton.Notification(NotificationSystem.NotificationType.Download);
                sendNotification = true;
            }
            if (slider.value <= targetProgress)
            {
                slider.value += fillSpeed * Time.deltaTime;
            }

            if (slider.value == 1)
            {
                slider.gameObject.SetActive(false);
                WorldCanvas.gameObject.SetActive(false);
                if (hackingObject != null)
                {
                    GameplayHandler.singleton.CompleteHack(hackingObject);
                    NotificationSystem.singleton.NotificationCallback(NotificationSystem.NotificationType.Download);
                    sendNotification = false;
                }
            }

            Progress(1);
        }
    }

    public void Progress(float newProgress)
    {
        targetProgress = slider.value + newProgress;
    }
}
