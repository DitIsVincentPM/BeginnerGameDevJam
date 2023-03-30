using UnityEngine;

public class DestroyAfterPlay : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
