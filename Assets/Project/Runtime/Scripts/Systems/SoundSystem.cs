using UnityEngine;
using System.Collections.Generic;

public class SoundSystem : StaticInstance<SoundSystem>
{
    [SerializeField]
    private List<AudioSource> _narratorSources;

    [SerializeField]
    private List<AudioSource> _alarmSources;

    [SerializeField]
    private AudioSource _musicSource;

    [SerializeField]
    private AudioSource _soundSource;

    [SerializeField]
    private AudioClip Robot;

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

        PlayMusic(Robot);
    }

    public void PauseAudio()
    {
        foreach (AudioSource narratorSource in _narratorSources)
        {
            narratorSource.Pause();
        }
        foreach (AudioSource alarmSource in _alarmSources)
        {
            alarmSource.Pause();
        }
        foreach (GameObject soundSource in GameObject.FindGameObjectsWithTag("SoundSource"))
        {
            soundSource.GetComponent<AudioSource>().Pause();
        }
    }

    public void UnPauseAudio()
    {
        foreach (AudioSource narratorSource in _narratorSources)
        {
            narratorSource.Play();
        }
        foreach (AudioSource alarmSource in _alarmSources)
        {
            alarmSource.Play();
        }
        foreach (GameObject soundSource in GameObject.FindGameObjectsWithTag("SoundSource"))
        {
            soundSource.GetComponent<AudioSource>().Play();
        }
    }

    public void RefreshNarratorSource()
    {
        _narratorSources.Clear();
        foreach (GameObject audioSource in GameObject.FindGameObjectsWithTag("NarratorSource"))
        {
            _narratorSources.Add(audioSource.GetComponent<AudioSource>());
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1)
    {
        AudioSource audioSource = Instantiate(_soundSource);
        audioSource.transform.position = position;
        audioSource.volume = volume;
        audioSource.clip = clip;
        audioSource.Play();
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
            narratorSource.volume = volume;
            narratorSource.clip = clip;
            narratorSource.loop = false;
            narratorSource.Play();
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }
}
