using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicControl : MonoBehaviour {
    [SerializeField] private AudioSource _dayMusicSource;
    [SerializeField] private AudioSource _nightMusicSource;
    [SerializeField] private AudioSource _soundEffectsSource;

    [SerializeField] private AudioMixerSnapshot _dayMusicSnapshot;
    [SerializeField] private AudioMixerSnapshot _nightMusicSnapshot;
    [SerializeField] private AudioMixerSnapshot _soundEffectsSnapshot;
    [SerializeField] private AudioMixerSnapshot _pausedSnapshot;

    [SerializeField] private AudioClip _dayMusic;
    [SerializeField] private AudioClip _nightMusic;
    [SerializeField] private AudioClip _nightSFX;
    [SerializeField] private AudioClip _daySFX;

    private bool isDay;

    private void Start()
    {
        Invoke("ChangeToDayMusic", 0.5f);
    }

    public void PauseMusic(bool isPaused)
    {
        if (isPaused)
        {
            StartCoroutine(SlowDown());
        }
        else
        {
            StartCoroutine(SpeedUp());
        }
    }

    IEnumerator SlowDown()
    {
        for (int i = 0; i < 50; i++)
        {
            _dayMusicSource.pitch -= 0.01f;
            _nightMusicSource.pitch -= 0.01f;
            _dayMusicSource.volume -= 0.02f;
            _nightMusicSource.volume -= 0.02f;
            yield return null;
        }
        _dayMusicSource.volume = 0f;
        _nightMusicSource.volume = 0f;
        yield break;
    }

    IEnumerator SpeedUp()
    {
        for (int i = 0; i < 50; i++)
        {
            _dayMusicSource.pitch += 0.01f;
            _nightMusicSource.pitch += 0.01f;
            _dayMusicSource.volume += 0.02f;
            _nightMusicSource.volume += 0.02f;
            yield return null;
        }
        _dayMusicSource.volume = 1f;
        _nightMusicSource.volume = 1f;
        yield break;
    }

    public void ChangeToNightMusic()
    {
        isDay = false;
        StartCoroutine(PlaySFX(_nightSFX));
    }
    public void ChangeToDayMusic()
    {
        isDay = true;
        StartCoroutine(PlaySFX(_daySFX));
    }

    private IEnumerator PlaySFX(AudioClip clip)
    {
        _soundEffectsSnapshot.TransitionTo(0.05f);
        _soundEffectsSource.clip = clip;
        _soundEffectsSource.Play();
        yield return new WaitForSeconds(1f);
        if (isDay)
        {
            _dayMusicSnapshot.TransitionTo(3f);
            _nightMusicSource.Stop();
            _dayMusicSource.clip = _dayMusic;
            _dayMusicSource.Play();
        }
        else
        {
            _nightMusicSnapshot.TransitionTo(3f);
            _dayMusicSource.Stop();
            _nightMusicSource.clip = _nightMusic;
            _nightMusicSource.Play();
        }
        yield break;
    }
}
