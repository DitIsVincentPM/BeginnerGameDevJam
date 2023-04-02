using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class MenuController : MonoBehaviour
{
    [Header("MainMenu")]
    [SerializeField]
    private GameObject canvasObject;

    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private GameObject camPlayer;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject otherCanvas;

    [SerializeField]
    private GameObject narratorCanvas;

    [Header("Other Menus")]
    [SerializeField]
    private GameObject optionMenu;

    [SerializeField]
    private GameObject graphicsMenu;

    [SerializeField]
    private GameObject soundMenu;

    [SerializeField]
    private GameObject controlMenu;

    [Header("Resolution")]
    public List<resItem> resolutions = new List<resItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;
    public TMP_Dropdown dropdown;

    [Header("Camera")]
    [SerializeField]
    CameraController camControler;

    [SerializeField] private InputActionReference escape;
    
    private void OnEnable() 
    {
        escape.action.performed += EscapePerformed;
    }


    private void OnDisable() 
    {
        escape.action.performed -= EscapePerformed;
    }

    private void EscapePerformed(InputAction.CallbackContext obj)
    {
        EscapePressed();
    }

    // Method to disable all child objects of the player
    void Start()
    {
        DisableComponentsOnTarget();
        otherCanvas.SetActive(false);
        narratorCanvas.SetActive(false);
    }

    void Update()
    {

    }

    private void EscapePressed()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsMenuActive())
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            DisableComponentsOnTarget();
            otherCanvas.SetActive(false);
            narratorCanvas.SetActive(false);
            SoundSystem.Instance.PauseAudio();
        }
    }

    private bool IsMenuActive()
    {
        if (
            optionMenu.activeSelf
            || graphicsMenu.activeSelf
            || soundMenu.activeSelf
            || controlMenu.activeSelf
            || pauseMenu.activeSelf
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DisableComponentsOnTarget()
    {
        // Get all components on the target object
        // Get all components on the target object and its children
        Component[] components = playerObject.GetComponentsInChildren<Component>();
        camControler.enabled = false;
        // Loop through the components and disable them (except for Transform and GameObject)
        foreach (Component component in components)
        {
            if (
                !(component is Transform)
                && !(component is Camera)
                && !(component is AudioListener)
                && !(component is CameraMainMenu)
            )
            {
                Behaviour behaviour = component as Behaviour;
                if (behaviour != null)
                {
                    behaviour.enabled = false;
                }
            }
        }
    }

    public void EnableComponentsOnTarget()
    {
        Component[] components = playerObject.GetComponentsInChildren<Component>();
        camControler.enabled = true;
        foreach (Component component in components)
        {
            if (
                !(component is Transform)
                && !(component is Camera)
                && !(component is AudioListener)
                && !(component is CameraMainMenu)
            )
            {
                Behaviour behaviour = component as Behaviour;
                if (behaviour != null)
                {
                    behaviour.enabled = true;
                }
            }
        }
    }

    public void ContinueBtn()
    {
        SoundSystem.Instance.UnPauseAudio();
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        otherCanvas.SetActive(true);
        EnableComponentsOnTarget();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        narratorCanvas.SetActive(true);
    }

    // Method to deactivate the canvas
    public void StartBtn()
    {
        Time.timeScale = 1f;
        mainMenu.SetActive(false);
        otherCanvas.SetActive(true);
        EnableComponentsOnTarget();
        narratorCanvas.SetActive(true);

        GameplayHandler.Instance.StartGame();
    }

    public void QuitGameBtn()
    {
        Application.Quit();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text =
            resolutions[selectedResolution].horizontal.ToString()
            + " x "
            + resolutions[selectedResolution].vertical.ToString();
    }

    public void ResLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResLabel();
    }

    public void SetBorderlessFullscreen()
    {
        Screen.SetResolution(
            Screen.currentResolution.width,
            Screen.currentResolution.height,
            FullScreenMode.FullScreenWindow
        );
    }

    public void ApplyGraphics()
    {
        Screen.SetResolution(
            resolutions[selectedResolution].horizontal,
            resolutions[selectedResolution].vertical,
            Screen.fullScreenMode
        );
        int selectedOptionIndex = dropdown.value;
        switch (selectedOptionIndex)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            default:
                break;
        }
    }

    [System.Serializable]
    public class resItem
    {
        public int horizontal,
            vertical;
    }

    public void ChangeNarratorVolume(Slider slider)
    {
        SoundSystem.Instance.NarratorVolume = slider.value;
        SoundSystem.Instance.ChangeVolume();
    }

    public void ChangeMusicVolume(Slider slider)
    {
        SoundSystem.Instance.MusicVolume = slider.value;
        SoundSystem.Instance.ChangeVolume();
    }

    public void ChangeSfxVolume(Slider slider)
    {
        SoundSystem.Instance.SfxVolume = slider.value;
        SoundSystem.Instance.ChangeVolume();
    }

    public void ChangeMasterVolume(Slider slider)
    {
        SoundSystem.Instance.MasterVolume = slider.value;
        SoundSystem.Instance.ChangeVolume();
    }
}
