using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private AudioSource _soundsSource;
    [SerializeField] private AudioSource _narratorSource;
    [SerializeField] private AudioSource _musicSource;

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1)
    {
        _soundsSource.transform.position = position;
        PlaySound(clip, volume);
    }

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        _soundsSource.PlayOneShot(clip, volume);
    }

    public void PlayNarrator(AudioClip clip, float volume = 1)
    {
        _narratorSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }
}
