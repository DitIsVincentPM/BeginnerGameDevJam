using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Gameobjects")]
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject camPlayer;

    [Header("Resolution")]
    public List<resItem> resolutions = new List<resItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;
    public Toggle fullscreenToggle;


    // Method to disable all child objects of the player
    void Start()
    {
        DisableComponentsOnTarget();
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void DisableComponentsOnTarget()
    {
        // Get all components on the target object
         // Get all components on the target object and its children
        Component[] components = playerObject.GetComponentsInChildren<Component>();

        // Loop through the components and disable them (except for Transform and GameObject)
        foreach (Component component in components)
        {
            if (!(component is Transform) && !(component is Camera) && !(component is AudioListener))
            {
                Behaviour behaviour = component as Behaviour;
                if (behaviour != null)
                {
                    behaviour.enabled = false;
                }
            }
        }
    }

    // Method to deactivate the canvas
    public void StartBtn()
    {
        canvasObject.SetActive(false);

        foreach (Transform child in playerObject.transform)
        {
            child.gameObject.SetActive(true);
        }

        
        Component[] components = playerObject.GetComponentsInChildren<Component>();

        // Loop through the components and disable them (except for Transform and GameObject)
        foreach (Component component in components)
        {
            if (!(component is Transform) && !(component is Camera) && !(component is AudioListener))
            {
                Behaviour behaviour = component as Behaviour;
                if (behaviour != null)
                {
                    behaviour.enabled = true;
                }
            }
        }
    }

    public void QuitGameBtn()
    {
        Application.Quit();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }
    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
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

    public void ApplyGraphics()
    {
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenToggle.isOn);
    }

    [System.Serializable]
    public class resItem
    {
        public int horizontal, vertical;
    }
}