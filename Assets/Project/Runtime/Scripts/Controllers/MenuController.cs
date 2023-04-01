using UnityEngine;

public class MenuController : MonoBehaviour
{
    [Header("StartBtn")]
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject camPlayer;

    


    // Method to disable all child objects of the player
    void Start()
    {
        DisableComponentsOnTarget();
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
}