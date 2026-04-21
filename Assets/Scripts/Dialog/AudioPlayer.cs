using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
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

    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private AudioSource _ambienceAudioSource;
    [SerializeField] private Animator _musicAudioAnimator;
    [SerializeField] private Animator _ambienceAudioAnimator;

    private void Awake()
    {
        Instance = this;
        PopulateAudioSources();
    }

    public void FadeOutAmbience(float speed = 1f)
    {
        FadeOutLoopingAudio(_ambienceAudioSource, _ambienceAudioAnimator, speed);
    }
    public void FadeOutMusic(float speed = 1f)
    {
        FadeOutLoopingAudio(_musicAudioSource, _musicAudioAnimator, speed);
    }
    public void PlayMusic(string path, float speed = 1f)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/"+path);
        PlayLoopingAudio(_musicAudioSource, _musicAudioAnimator, clip, speed);
    }
    public void PlayAmbience(string path, float speed = 1f)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + path);
        PlayLoopingAudio(_ambienceAudioSource, _ambienceAudioAnimator, clip, speed);
    }
    public async void PlayLoopingAudio(AudioSource source, Animator animator, AudioClip clip, float speed = 1f)
    {
        animator.speed = speed;
        if (source.isPlaying) 
        {
            animator.SetTrigger("FadeOut");
            await Task.Delay((int)(1000 / speed));
        }
        source.clip = clip;
        source.Play();
        animator.SetTrigger("FadeIn");
    }
    private async void FadeOutLoopingAudio(AudioSource source, Animator animator, float speed = 1f)
    {
        animator.speed = speed;
        animator.SetTrigger("FadeOut");
        await Task.Delay((int)(1000 / speed));
        if (source == null) return;
        source?.Stop();
        animator.speed = 1000f;
        animator.SetTrigger("FadeIn");
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
