using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class Interact : StaticInstance<Interact>
{
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

    [SerializeField]
    private InputActionReference interactKey;

    private void OnEnable()
    {
        interactKey.action.performed += InteractPerformed;
    }

    private void OnDisable()
    {
        interactKey.action.performed += InteractPerformed;
    }

    private void InteractPerformed(InputAction.CallbackContext obj)
    {
        foreach (Interactable interact in intractables)
        {
            if (interact.interactText.enabled == true)
            {
                Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, _raycastDistance, _layerMask))
                {
                    switch (hit.collider.gameObject.name)
                    {
                        case "Monitor":
                            GameplayHandler.Instance.StartHacking(hit.collider.gameObject);
                            break;
                        case "CDReader":
                            GameplayHandler.Instance.StartDiskUpload(hit.collider.gameObject);
                            break;
                        case "Button":
                            PuzzleHandler.Instance.ButtonPress();
                            break;
                        case "Screen1":
                            PuzzleHandler.Instance.Hacked(1);
                            break;
                        case "Screen2":
                            PuzzleHandler.Instance.Hacked(2);
                            break;
                        case "Screen3":
                            PuzzleHandler.Instance.Hacked(3);
                            break;
                        case "Screen4":
                            PuzzleHandler.Instance.Hacked(4);
                            break;
                        case "Screen5":
                            PuzzleHandler.Instance.Hacked(5);
                            break;
                        case "Screen6":
                            PuzzleHandler.Instance.Hacked(6);
                            break;
                    }
                }
            }
        }
    }

    private void Start()
    {
        foreach (Interactable interact in intractables)
        {
            interact.interactText.text = interact.textValue;
        }
        _camera = Camera.main;
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
