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

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * _raycastDistance);

        if (Physics.Raycast(ray, out hit, _raycastDistance, _layerMask))
        {
            _interactText.text = "Press [E] to read the note.";
            Debug.Log("Hit");
        }
    }
}
