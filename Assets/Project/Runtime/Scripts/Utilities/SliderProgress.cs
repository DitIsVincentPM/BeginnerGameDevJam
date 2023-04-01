using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderProgress : MonoBehaviour
{
    private float targetProgress = 0;
    private float fillSpeed = 0.1f;
    private GameObject obj;

    // Set the progressbars target
    public void SetProgress(GameObject objects, int target)
    {
        obj = objects;
        targetProgress = target;
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
                        GameplayHandler.Instance.StopHacking(obj);
                        break;
                    case "Server":
                        GameplayHandler.Instance.DownloadingDisk(obj);
                        break;
                    case "CDReader": 
                        GameplayHandler.Instance.UploadedDisk(obj);
                        break;
                }
            }
        }
    }
}
