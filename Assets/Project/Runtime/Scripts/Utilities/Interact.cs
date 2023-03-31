using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private float _raycastDistance = 10f;
    [SerializeField] private LayerMask _layerMask;
    private Camera _camera;

    [Header("Text")]
    [SerializeField] private TMP_Text _interactText;
    [SerializeField] private string textValue;
    
    [Header("ProgressBar")]
    [SerializeField] private SliderProgress _hackingProgress;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Awake() {
        _interactText.text = textValue;
    }

    void Update()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * _raycastDistance);

        _interactText.enabled = Physics.Raycast(ray, out hit, _raycastDistance, _layerMask);    

        //opent slider en activeert die
        if(Input.GetKeyDown(KeyCode.E) && Physics.Raycast(ray, out hit, _raycastDistance, _layerMask))
        {
            _hackingProgress.StartHacking(hit.collider.gameObject);
        }
    }
}
