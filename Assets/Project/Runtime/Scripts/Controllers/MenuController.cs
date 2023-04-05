using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField]
    private GameObject gameOver;

    [Header("Resolution")]
    public List<resItem> resolutions = new List<resItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;
    public TMP_Dropdown dropdown;

    [Header("Graphic Tier")]
    public TMP_Dropdown dropdown2;

    [Header("Sound Settings")]
    public Slider MasterSlider;
    public Slider SfxSlider;
    public Slider MusicSlider;
    public Slider NarratorSlider;

    [Header("Camera")]
    [SerializeField]
    CameraController camControler;

    [SerializeField]
    private InputActionReference escape;

    public void EnableRestartMenu()
    {
        gameOver.SetActive(true);
    }

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
        selectedResolution = StoreSettings.Instance.Resolution;
        dropdown.value = StoreSettings.Instance.Window;
        dropdown2.value = StoreSettings.Instance.Graphics;
        SfxSlider.value = StoreSettings.Instance.SfxVolume;
        SoundSystem.Instance.SfxVolume = StoreSettings.Instance.SfxVolume;
        MusicSlider.value = StoreSettings.Instance.MusicVolume;
        SoundSystem.Instance.MusicVolume = StoreSettings.Instance.MusicVolume;
        MasterSlider.value = StoreSettings.Instance.MasterVolume;
        SoundSystem.Instance.MasterVolume = StoreSettings.Instance.MasterVolume;
        NarratorSlider.value = StoreSettings.Instance.NarratorVolume;
        SoundSystem.Instance.NarratorVolume = StoreSettings.Instance.NarratorVolume;

        Screen.SetResolution(
            resolutions[StoreSettings.Instance.Resolution].horizontal,
            resolutions[StoreSettings.Instance.Resolution].vertical,
            Screen.fullScreenMode
        );

        switch (StoreSettings.Instance.Window)
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

        switch (StoreSettings.Instance.Graphics)
        {
            case 0:
                QualitySettings.SetQualityLevel(2, true);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1, true);
                break;
            case 2:
                QualitySettings.SetQualityLevel(0, true);
                break;
            default:
                break;
        }
        DisableComponentsOnTarget();
        otherCanvas.SetActive(false);
        narratorCanvas.SetActive(false);
    }

    void Update() { }

    private void EscapePressed()
    {
        if (!IsMenuActive())
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

    public void RestartBtn()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);

        otherCanvas.SetActive(false);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
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
        StoreSettings.Instance.Resolution = selectedResolution;
        Screen.SetResolution(
            resolutions[selectedResolution].horizontal,
            resolutions[selectedResolution].vertical,
            Screen.fullScreenMode
        );
        StoreSettings.Instance.Window = dropdown.value;
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

        StoreSettings.Instance.Graphics = dropdown2.value;
        int selectedOptionIndex2 = dropdown2.value;
        switch (selectedOptionIndex2)
        {
            case 0:
                QualitySettings.SetQualityLevel(2, true);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1, true);
                break;
            case 2:
                QualitySettings.SetQualityLevel(0, true);
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
        StoreSettings.Instance.NarratorVolume = slider.value;
        SoundSystem.Instance.NarratorVolume = slider.value;
        SoundSystem.Instance.ChangeVolume();
    }

    public void ChangeMusicVolume(Slider slider)
    {
        StoreSettings.Instance.MusicVolume = slider.value;
        SoundSystem.Instance.MusicVolume = slider.value;
        SoundSystem.Instance.ChangeVolume();
    }

    public void ChangeSfxVolume(Slider slider)
    {
        StoreSettings.Instance.SfxVolume = slider.value;
        SoundSystem.Instance.SfxVolume = slider.value;
        SoundSystem.Instance.ChangeVolume();
    }

    public void ChangeMasterVolume(Slider slider)
    {
        StoreSettings.Instance.MasterVolume = slider.value;
        SoundSystem.Instance.MasterVolume = slider.value;
        SoundSystem.Instance.ChangeVolume();
    }
}
