using UnityEngine;
using System.Collections.Generic;

public class SoundSystem : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _narratorSources;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundSource;

    private void Start()
    {
        foreach (GameObject audioSource in GameObject.FindGameObjectsWithTag("NarratorSource"))
        {
            _narratorSources.Add(audioSource.GetComponent<AudioSource>());
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1)
    {
        AudioSource audioSource = Instantiate(_soundSource);
        audioSource.transform.position = position;
        audioSource.PlayOneShot(clip, volume);
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
