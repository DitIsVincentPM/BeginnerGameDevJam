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
    public GameObject hackingObject;

    [Header("Sound Clips")]

    [SerializeField]
    private AudioClip hackingSound;

    public void StartHacking(GameObject obj)
    {
        hackingObject = obj;
        NotificationSystem.singleton.Notification(NotificationSystem.NotificationType.Hacking);
        targetProgress = 1;
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
        GameplayHandler.singleton.CompleteHack(hackingObject);
        UISystem.singleton.NotificationSlider.gameObject.SetActive(false);
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
                StopHacking();
            }
        }
    }
}
