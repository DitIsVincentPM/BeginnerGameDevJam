using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class Interact : MonoBehaviour
{
    public static new Interact singleton { get; set; }

    [Header("Raycast")]
    [SerializeField]
    private float _raycastDistance = 10f;

    [SerializeField]
    private LayerMask _layerMask;
    private Camera _camera;

    [System.Serializable]
    public class Interactable
    {
        public string name;
        public TMP_Text interactText;
        public string textValue;
        public bool isEnabled;

        public Interactable(string name, string textValue, TMP_Text interactText, bool isEnabled)
        {
            this.name = name;
            this.textValue = textValue;
            this.interactText = interactText;
            this.isEnabled = isEnabled;
        }
    }

    [Header("Interactables")]
    public List<Interactable> intractables;

    [Header("ProgressBar")]
    [SerializeField]
    private SliderProgress _progress;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Awake()
    {
        singleton = this;

        foreach (Interactable interact in intractables)
        {
            interact.interactText.text = interact.textValue;
        }
    }

    public bool SetInteractable(string name, bool state = false)
    {
        foreach (Interactable interact in intractables)
        {
            if (interact.name == name)
            {
                interact.isEnabled = state;
                interact.interactText.enabled = false;
                return true;
            }
        }
        return false;
    }

    void Update()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * _raycastDistance);

        if (Physics.Raycast(ray, out hit, _raycastDistance, _layerMask))
        {
            foreach (Interactable interact in intractables)
            {
                if (interact.isEnabled && hit.collider.gameObject.name == interact.name)
                {
                    if (interact.interactText.enabled == false)
                        interact.interactText.enabled = true;

                    if (
                        Input.GetKeyDown(KeyCode.E)
                        && Physics.Raycast(ray, out hit, _raycastDistance, _layerMask)
                    )
                    {
                        switch (hit.collider.gameObject.name)
                        {
                            case "Monitor":
                                _progress.StartHacking(hit.collider.gameObject);
                                break;
                            case "CDReader":
                                _progress.StartDiskUpload(hit.collider.gameObject);
                                break;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (Interactable interact in intractables)
            {
                if (interact.interactText.enabled == true)
                    interact.interactText.enabled = false;
            }
        }
    }
}
