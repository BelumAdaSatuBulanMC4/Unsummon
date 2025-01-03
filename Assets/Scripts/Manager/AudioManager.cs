using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio References")]
    [SerializeField] private AudioSource musicAudioSource;
    private AudioSource[] sfxAudioSources;


    [Header("AudioClip References")]
    [SerializeField] private AudioClip musicGamePlay;
    [SerializeField] private AudioClip musicMenuLobby;

    private string currentSceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        sfxAudioSources = GetAllSFXAudioSource();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"OnSceneLoaded AudioManager - CurrentScene: {currentSceneName}");
        if (currentSceneName == "GameStory")
        {
            musicAudioSource.volume = 0f;
            for (int i = 0; i < sfxAudioSources.Length; i++) sfxAudioSources[i].volume = 0f;
        }
        else if (currentSceneName == "GamePlayNew" || currentSceneName == "GamePlayKid")
        {
            musicAudioSource.clip = musicGamePlay;
            musicAudioSource.Play();
        }
        else
        {
            musicAudioSource.clip = musicMenuLobby;
            if (!musicAudioSource.isPlaying) musicAudioSource.Play();
            musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            for (int i = 0; i < sfxAudioSources.Length; i++)
            {
                sfxAudioSources[i].volume = PlayerPrefs.GetFloat("SFXVolume", 0.3f);
            }

        }
    }

    public AudioSource GetMusicAudioSource()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        AudioSource musicAudioSource = audioManager.transform.Find("AudioMusic")?.GetComponent<AudioSource>();
        return musicAudioSource;
    }

    public AudioSource GetSFXAudioSource(int index)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        AudioSource[] audioSources = audioManager.transform.Find("AudioSFX")?.GetComponents<AudioSource>();
        return audioSources[index];
    }

    public AudioSource[] GetAllSFXAudioSource()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        AudioSource[] audioSources = audioManager.transform.Find("AudioSFX")?.GetComponents<AudioSource>();
        return audioSources;
    }

}
