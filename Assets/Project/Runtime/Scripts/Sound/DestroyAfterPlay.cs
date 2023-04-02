using UnityEngine;

public class DestroyAfterPlay : MonoBehaviour
{
    private AudioSource _audioSource;
    private float currentLength = 0;
    private float timer = 0;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Start() {
         currentLength = _audioSource.clip.length;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > currentLength)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
