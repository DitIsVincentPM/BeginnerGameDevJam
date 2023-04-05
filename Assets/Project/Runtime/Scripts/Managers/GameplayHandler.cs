using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayHandler : StaticInstance<GameplayHandler>
{
    [Header("Map Parts")]
    [SerializeField]
    public GameObject mapPuzzle1;

    [SerializeField]
    public GameObject mapPuzzle2;

    [SerializeField]
    public GameObject mapPuzzle3;

    [SerializeField]
    public GameObject mapPuzzle4;

    [Header("Game Value's")]
    [SerializeField]
    public int currentPuzzle = 0;

    [SerializeField]
    private LayerMask _layerMask;

    [Header("Materials")]
    [SerializeField]
    private Material outlineMat;

    [SerializeField]
    private Material outlineMask;

    [Header("Sound Effects")]
    [SerializeField]
    private AudioClip powerUpGrid;

    [SerializeField]
    private AudioClip failSound;

    [SerializeField]
    private AudioClip hackingSound;

    [SerializeField]
    private AudioClip uploadSound;

    [SerializeField]
    private AudioClip alarm;

    [Header("Storage")]
    [SerializeField]
    Animator UIanim;

    [SerializeField]
    private AudioClip MainTheme;

    [SerializeField]
    private AudioClip HighAlert;

    [SerializeField]
    private AudioClip Chasing;

    [SerializeField]
    private AudioClip Robot;

    [SerializeField]
    NarratorSystem narratorSystem;

    [SerializeField]
    RaycastController raycastController;
    public GameObject CDReader;

    [Space(10)]
    [SerializeField]
    DoorController FirstDoor;

    [SerializeField]
    DoorController HallwayDoor;

    [SerializeField]
    DoorController LiftDoor;

    [SerializeField]
    CameraMainMenu mainCamScript;

    [Space(10)]
    [SerializeField]
    private List<LichtFlasher> _lights;

    [SerializeField]
    private List<GameObject> ServersRoom;

    private RaycastHit oldRaycast = default(RaycastHit);
    private float timer;
    private float timerTime;

    [Header("InputSystem")]
    [SerializeField]
    private InputActionReference _interact;

    void Start()
    {
        // Disable Map Parts that are not visible
        mapPuzzle2.SetActive(false);
        mapPuzzle3.SetActive(false);
        mapPuzzle4.SetActive(false);
        currentPuzzle = -1;

        foreach (GameObject source in GameObject.FindGameObjectsWithTag("AlarmSource"))
        {
            _lights.Add(source.transform.GetComponentInChildren<LichtFlasher>());
        }
        DisarmAlarm();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
                if (_interact.action.triggered)
                {
                    if (hit.collider.gameObject.GetComponent<ServerDiskHandler>() != null)
                    {
                        NotificationSystem.Instance.uploadDownload.transform
                            .GetChild(0)
                            .GetComponent<SliderProgress>()
                            .SetProgress(hit.collider.gameObject, 1);
                        NotificationSystem.Instance.Notification(
                            NotificationSystem.NotificationType.Download,
                            "Disk Content"
                        );
                    }
                    else
                    {
                        SoundSystem.Instance.PlaySound(
                            failSound,
                            hit.collider.gameObject.transform.position,
                            0.3f
                        );
                        NotificationSystem.Instance.Notification(
                            NotificationSystem.NotificationType.Error,
                            " 404\r\nNo disk found in server"
                        );
                    }
                }
            }
            oldRaycast = hit;
        }
    }

    //-------------------------------------------------------------//
    // Gameplay Functions (Volgorde)
    //-------------------------------------------------------------//

    public void StartGame()
    {
        mainCamScript.StartGame();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Disable Map Parts that are not visible
        mapPuzzle2.SetActive(false);
        mapPuzzle3.SetActive(false);
        currentPuzzle = 0;

        // Give disk to random Server
        int randomnumber = Random.Range(0, GameObject.FindGameObjectsWithTag("Server").Length);
        GameObject.FindGameObjectsWithTag("Server")[randomnumber].AddComponent<ServerDiskHandler>();
        DisarmAlarm();
        narratorSystem.SayVoiceLine(narratorSystem.voiceLines[0]);
    }

    // Activate's when clicking E on Monitor
    public void StartHacking(GameObject objects)
    {
        NotificationSystem.Instance.uploadDownload.transform
            .GetChild(0)
            .GetComponent<SliderProgress>()
            .SetProgress(objects, 1);
        Interact.Instance.SetInteractable("Monitor", false);
        NotificationSystem.Instance.Notification(NotificationSystem.NotificationType.Hacking);
        SoundSystem.Instance.PlaySound(
            hackingSound,
            FindFirstObjectByType<PlayerController>().gameObject.transform.position
        );
    }

    // Gets called on the callback when hacking of the Monitor is done
    public void StopHacking(GameObject objects)
    {
        NotificationSystem.Instance.NotificationCallback(
            NotificationSystem.NotificationType.Hacking
        );
        narratorSystem.SayVoiceLine(narratorSystem.voiceLines[1]);
        FirstDoor.activationState = DoorController.DoorActivation.Proximity;
        ArmAlarm();
        SoundSystem.Instance.PlayMusic(HighAlert);
        currentPuzzle = 1;
        UISystem.Instance.NotificationSlider.gameObject.SetActive(false);
    }

    // Gets called when you found the right server
    public void DownloadingDisk(GameObject objects)
    {
        NotificationSystem.Instance.NotificationCallback(
            NotificationSystem.NotificationType.Download
        );
        NarratorSystem.Instance.SayVoiceLine(NarratorSystem.Instance.voiceLines[2]);
        GameplayHandler.Instance.CDReader.GetComponent<Outline>().enabled = true;
        GameObject.FindObjectOfType<PlayerController>().inInventory.Add("Disk");
        Interact.Instance.SetInteractable("CDReader", true);
        GameplayHandler.Instance.currentPuzzle = 2;
        NotificationSystem.Instance.uploadDownload.transform
            .GetChild(0)
            .GetComponent<SliderProgress>()
            .SetProgress(objects, 1);
    }

    // This gets called when disk is uploading to the disk reader
    public void StartDiskUpload(GameObject objects)
    {
        NotificationSystem.Instance.uploadDownload.transform
            .GetChild(0)
            .GetComponent<SliderProgress>()
            .SetProgress(objects, 1);
        Interact.Instance.SetInteractable("CDReader", false);
        GameplayHandler.Instance.CDReader.GetComponent<Outline>().enabled = false;
        NotificationSystem.Instance.Notification(
            NotificationSystem.NotificationType.Upload,
            "Disk Content"
        );
        SoundSystem.Instance.PlaySound(uploadSound, objects.transform.position);
    }

    // Gets called when Disk is uploaded to the disk reader
    public void UploadedDisk(GameObject objects)
    {
        OpenMapPart("Puzzle2");

        NarratorSystem.Instance.SayVoiceLine(NarratorSystem.Instance.voiceLines[3]);
        GameplayHandler.Instance.DisarmAlarm();
        NotificationSystem.Instance.NotificationCallback(
            NotificationSystem.NotificationType.Upload
        );
        GameplayHandler.Instance.currentPuzzle = 3;
        NotificationSystem.Instance.uploadDownload.transform
            .GetChild(0)
            .GetComponent<SliderProgress>()
            .SetProgress(objects, 0);
        HallwayDoor.activationState = DoorController.DoorActivation.Proximity;
        SoundSystem.Instance.PlayMusic(MainTheme);
    }

    public void ServersPowerdOn()
    {
        if (GameplayHandler.Instance.currentPuzzle != 3)
            return;
        foreach (GameObject server in ServersRoom)
        {
            server.GetComponent<Renderer>().materials[2].color = new Color(0.08220265f, 1f, 0);
        }

        SoundSystem.Instance.PlaySound(
            powerUpGrid,
            FindFirstObjectByType<PlayerController>().gameObject.transform.position
        );
        GameplayHandler.Instance.currentPuzzle = 4;
        NarratorSystem.Instance.SayVoiceLine(NarratorSystem.Instance.voiceLines[4]);
        LiftDoor.activationState = DoorController.DoorActivation.Proximity;
    }

    public void EndGame() {
        UIanim.SetBool("EndGame", true);
    }

    //-------------------------------------------------------------//
    // Other Functions
    //-------------------------------------------------------------//
    public void CloseMapPart(string map)
    {
        switch (map)
        {
            case "Puzzle1":
                HallwayDoor.transform.parent = mapPuzzle2.transform;
                HallwayDoor.activationState = DoorController.DoorActivation.StayClosed;
                mapPuzzle1.SetActive(false);
                SoundSystem.Instance.RefreshNarratorSource();

                break;
            case "Puzzle2":
                mapPuzzle2.SetActive(false);
                SoundSystem.Instance.RefreshNarratorSource();

                break;
            case "Puzzle3":
                mapPuzzle3.SetActive(false);
                SoundSystem.Instance.RefreshNarratorSource();

                break;
            case "Puzzle4":
                mapPuzzle4.SetActive(false);
                SoundSystem.Instance.RefreshNarratorSource();

                break;
        }
    }

    public void OpenMapPart(string map)
    {
        switch (map)
        {
            case "Puzzle1":
                mapPuzzle1.SetActive(true);
                SoundSystem.Instance.RefreshNarratorSource();

                break;
            case "Puzzle2":
                mapPuzzle2.SetActive(true);
                SoundSystem.Instance.RefreshNarratorSource();

                break;
            case "Puzzle3":
                mapPuzzle3.SetActive(true);
                SoundSystem.Instance.RefreshNarratorSource();

                break;
            case "Puzzle4":
                mapPuzzle4.SetActive(true);
                SoundSystem.Instance.RefreshNarratorSource();

                break;
        }
    }

    public void DisarmAlarm()
    {
        foreach (LichtFlasher light in _lights)
        {
            light.GetComponentInChildren<Light>().color = new Color(1, 1, 1);
            light.endColor = new Color(1, 1, 1);
            light.startColor = new Color(1, 1, 1);
            light.transform.GetComponentInChildren<Light>().intensity = 4;
        }
        SoundSystem.Instance.StopAlarm();
    }

    public void ArmAlarm()
    {
        foreach (LichtFlasher light in _lights)
        {
            light.endColor = Color.red;
            light.startColor = new Color(0.78f, 0.1945087f, 0.1993889f);
            light.transform.GetComponentInChildren<Light>().intensity = 5;
        }
        SoundSystem.Instance.PlayAlarm(alarm, 0.1f);
    }
}
