using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    private List<AudioSource> _audioSources = new();
    public static AudioPlayer Instance;
    [SerializeField] private AudioMixerGroup _masterMixerGroup;
    [SerializeField] private AudioMixerGroup _soundEffectsMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _ambienceMixerGroup;

    private void Awake()
    {
        Instance = this;
        PopulateAudioSources();
    }

    public void PlayOneShot(string path)
    {
        AudioClip clip = Resources.Load<AudioClip>(path);
        PlayOneShot(clip);
    }
    public void PlayOneShot(AudioClip clip)
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = clip;
                audioSource.outputAudioMixerGroup = _soundEffectsMixerGroup;
                audioSource.Play();
                return;
            }
        }
        _audioSources[0].clip = clip;
        _audioSources[0].outputAudioMixerGroup = _soundEffectsMixerGroup;
        _audioSources[0].Play();
    }

    private void PopulateAudioSources(int number = 10)
    {
        if (_audioSources.Count != 0)
        {
            Debug.LogWarning("AudioPlayer already has AudioSource(s). Destroying all and repopulating...");
            foreach (AudioSource audioSource in _audioSources) { Destroy(audioSource); }
            _audioSources.Clear();
        }

        for (int i = 0; i < number; i++) { _audioSources.Add(gameObject.AddComponent<AudioSource>()); }
    }
}
