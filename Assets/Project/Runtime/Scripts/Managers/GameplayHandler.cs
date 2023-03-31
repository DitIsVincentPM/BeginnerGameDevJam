using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayHandler : MonoBehaviour
{
    public static new GameplayHandler singleton { get; set; }

    [Header("Game Value's")]
    [SerializeField]
    public int currentPuzzle = 0;

    [SerializeField]
    private LayerMask _layerMask;

    [Header("Imports")]
    [SerializeField]
    private List<LichtFlasher> _lights;

    [SerializeField]
    NarratorSystem narratorSystem;

    [SerializeField]
    DoorController FirstDoor;

    [SerializeField]
    private AudioClip alarm;

    [SerializeField]
    RaycastController raycastController;

    [Header("Materials")]
    [SerializeField]
    private Material outlineMat;

    [SerializeField]
    private Material outlineMask;

    [Header("Sound Effects")]
    [SerializeField]
    private AudioClip failSound;

    [Header("Storage")]
    public GameObject CDReader;
    private RaycastHit oldRaycast = default(RaycastHit);
    private float timer;
    private float timerTime;

    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        currentPuzzle = 0;
        // Give disk to random Server
        int randomnumber = Random.Range(0, GameObject.FindGameObjectsWithTag("Server").Length);
        GameObject.FindGameObjectsWithTag("Server")[randomnumber].AddComponent<ServerDiskHandler>();

        foreach (GameObject source in GameObject.FindGameObjectsWithTag("AlarmSource"))
        {
            _lights.Add(source.transform.GetComponentInChildren<LichtFlasher>());
        }
        DisarmAlarm();
        narratorSystem.SayVoiceLine(narratorSystem.voiceLines[0]);
    }

    void Update()
    {
        // TimerSystem
        if (timerTime != 0)
        {
            timer += Time.deltaTime;
        }
        if (timer > timerTime)
        {
            if (oldRaycast.collider != null)
                if (oldRaycast.collider.gameObject.GetComponent<Outline>() != null)
                    oldRaycast.collider.gameObject.GetComponent<Outline>().enabled = false;
            timerTime = 0;
            timer = 0;
        }

        // Check Server Disk, Outline
        if (currentPuzzle == 1)
        {
            RaycastHit hit = raycastController.GetRaycastHit(10f, _layerMask);
            if (hit.collider == null)
                return;

            if (oldRaycast.collider != null && oldRaycast.collider != hit.collider)
                if (oldRaycast.collider.gameObject.GetComponent<Outline>() != null)
                    oldRaycast.collider.gameObject.GetComponent<Outline>().enabled = false;
            timerTime = 0.1f;

            if (hit.collider.gameObject.tag == "Server")
            {
                if (hit.collider.gameObject.GetComponent<Outline>() != null)
                {
                    var outline = hit.collider.gameObject.GetComponent<Outline>();
                    outline.enabled = true;
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (hit.collider.gameObject.GetComponent<ServerDiskHandler>() != null)
                    {
                        NotificationSystem.singleton.uploadDownload.transform.GetChild(0).GetComponent<SliderProgress>().SetProgress(hit.collider.gameObject, 1);
                        NotificationSystem.singleton.Notification(
                            NotificationSystem.NotificationType.Download, "Disk Content"
                        );
                    }
                    else
                    {
                        SoundSystem.singleton.PlaySound(failSound, hit.collider.gameObject.transform.position, 0.3f);
                        NotificationSystem.singleton.Notification(
                            NotificationSystem.NotificationType.Error,
                            " 404\r\nNo disk found in server"
                        );
                    }
                }
            }
            oldRaycast = hit;
        }
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
                currentPuzzle = 1;
                break;
        }
    }
}
