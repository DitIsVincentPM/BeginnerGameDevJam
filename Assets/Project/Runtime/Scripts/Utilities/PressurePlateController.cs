using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    [Header("Activated Object")]
    [SerializeField] private GameObject _objectToActivate;
    [SerializeField] private bool _isObjectActiveOnStart = false;

    private void Start()
    {
        _objectToActivate.SetActive(_isObjectActiveOnStart);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PressureBox"))
        {
            _objectToActivate.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PressureBox"))
        {
            _objectToActivate.SetActive(false);
        }
    }
}