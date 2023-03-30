using UnityEngine;
using System.Collections.Generic;

public class SoundSystem : MonoBehaviour
{
    public static new SoundSystem singleton { get; set; }

    [SerializeField]
    private List<AudioSource> _narratorSources;

    [SerializeField]
    private List<AudioSource> _alarmSources;

    [SerializeField]
    private AudioSource _musicSource;

    [SerializeField]
    private AudioSource _soundSource;

    [SerializeField]
    private AudioClip mainTheme;

    void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        foreach (GameObject audioSource in GameObject.FindGameObjectsWithTag("NarratorSource"))
        {
            _narratorSources.Add(audioSource.GetComponent<AudioSource>());
        }

        foreach (GameObject audioSource in GameObject.FindGameObjectsWithTag("AlarmSource"))
        {
            _alarmSources.Add(audioSource.GetComponent<AudioSource>());
        }

        PlayMusic(mainTheme);
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1)
    {
        AudioSource audioSource = Instantiate(_soundSource);
        audioSource.transform.position = position;
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayAlarm(AudioClip clip, float volume = 1)
    {
        foreach (AudioSource alarmSource in _alarmSources)
        {
            alarmSource.clip = clip;
            alarmSource.volume = volume;
            alarmSource.Play();
        }
    }

    public void StopAlarm()
    {
        foreach (AudioSource alarmSource in _alarmSources)
        {
            alarmSource.Stop();
        }
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
