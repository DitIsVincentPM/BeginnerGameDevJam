using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    [Header("Activated Object")]
    [SerializeField]
    private GameObject _objectToActivate;

    [SerializeField]
    private bool _isObjectActiveOnStart = false;

    [Header("Animation")]
    [SerializeField]
    public Animator animator;

    [Header("Settings")]
    [SerializeField] private string compareTag;

    [SerializeField] private ObjectPickupAbilityState objectPickup;

    private void Start()
    {
        if (_objectToActivate != null)
            _objectToActivate.SetActive(_isObjectActiveOnStart);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(compareTag))
        {
            if (_objectToActivate != null)
                _objectToActivate.SetActive(true);

            GameplayHandler.Instance.ServersPowerdOn();
            animator.SetBool("pressed", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(compareTag))
        {
            if (_objectToActivate != null)
                _objectToActivate.SetActive(false);

            Invoke("ResetAnimation", 0.5f);
        }
    }

    private void ResetAnimation()
    {
        animator.SetBool("pressed", false);
    }
}
