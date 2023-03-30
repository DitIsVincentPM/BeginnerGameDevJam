using UnityEngine;
using System.Collections.Generic;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _narratorSources;
    [SerializeField] private AudioSource _soundsSource;
    [SerializeField] private AudioSource _musicSource;

    private void Start()
    {
        foreach (GameObject audioSource in GameObject.FindGameObjectsWithTag("NarratorSource"))
        {
            _narratorSources.Add(audioSource.GetComponent<AudioSource>());
        }
    }

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
        foreach (AudioSource narratorSource in _narratorSources)
        {
            narratorSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }
}
