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
    [SerializeField] private AudioSource sfxAudioSource;
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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"OnSceneLoaded AudioManager - CurrentScene: {currentSceneName}");
        if (currentSceneName == "GameStory")
        {
            musicAudioSource.volume = 0f;
            sfxAudioSource.volume = 0f;
        }
        else if (currentSceneName == "GamePlayNew" || currentSceneName == "GamePlayKid") musicAudioSource.volume = 0f;
        else
        {
            musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            sfxAudioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.3f); ;
        }
    }

}
