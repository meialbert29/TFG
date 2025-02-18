using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceBehaviour : MonoBehaviour
{
    private EnvironmentController environmentController;

    public AudioSource audioSource;          // Component that will play the music
    public AudioSource rainAudioSource;      // Component that will play the storm sound
    public AudioClip[] musicClips;           // List of songs
    public KeyCode[] musicKeys;              // List of keys associated with the songs
    public AudioClip[] rainClips;            // List of storm sounds
    public KeyCode[] rainKeys;               // List of keys associated with the storm sounds

    public float fadeDuration = 1f;          // Duration of fade in/out

    void Start()
    {

        environmentController = FindAnyObjectByType<EnvironmentController>();
        if (environmentController == null) Debug.Log("EnvironmentController Script not found");

        if (musicClips.Length > 0 && audioSource != null)
        {
            PlayMusic(0);  // Start with the first song
        }
        else
        {
            Debug.LogError("No music clips found or the AudioSource is not assigned.");
        }

        if (rainClips.Length > 0 && rainAudioSource != null)
        {
            rainAudioSource.loop = true;
        }
    }

    void Update()
    {

        if (!environmentController.automaticMode)
        {
            // Check the keys to change the music sounds
            for (int i = 0; i < musicKeys.Length; i++)
            {
                if (Input.GetKeyDown(musicKeys[i]))
                {
                    StartCoroutine(FadeOutAndChangeMusic(i));
                }
            }
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                rainAudioSource.Stop();
            }
            else
            {
                // Check the keys to change the storm sounds
                for (int i = 0; i < rainKeys.Length; i++)
                {
                    if (Input.GetKeyDown(rainKeys[i]))
                    {
                        StartCoroutine(FadeOutAndChangeRainAudio(i));
                    }
                }
            }
        }

    }

    public IEnumerator FadeOutAndChangeMusic(int index)
    {
        yield return StartCoroutine(FadeOut(audioSource));
        PlayMusic(index);
        yield return StartCoroutine(FadeIn(audioSource));
    }

    public IEnumerator FadeOutAndChangeRainAudio(int index)
    {
        yield return StartCoroutine(FadeOut(rainAudioSource));
        PlayRainAudio(index);
        yield return StartCoroutine(FadeIn(rainAudioSource));
    }

    public void PlayMusic(int index)
    {
        if (index >= 0 && index < musicClips.Length && audioSource != null)
        {
            audioSource.clip = musicClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Index out of range or the AudioSource is not assigned.");
        }
    }

    public void PlayRainAudio(int index)
    {
        if (index >= 0 && index < rainClips.Length && rainAudioSource != null)
        {
            rainAudioSource.clip = rainClips[index];
            rainAudioSource.volume = 0.5f;
            rainAudioSource.Play();

        }
        else
        {
            Debug.LogWarning("Index out of range or the rain AudioSource is not assigned.");
        }
    }

    public void StopRaindAudio()
    {
        rainAudioSource.Stop();
    }

    IEnumerator FadeOut(AudioSource source)
    {
        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }

    IEnumerator FadeIn(AudioSource source)
    {
        source.volume = 0f;
        float targetVolume = 1f;

        if (source == rainAudioSource)
            targetVolume = 0.3f;

        while (source.volume < targetVolume)
        {
            source.volume += Time.deltaTime / fadeDuration;
            yield return null;
        }

        source.volume = targetVolume;
    }
}
