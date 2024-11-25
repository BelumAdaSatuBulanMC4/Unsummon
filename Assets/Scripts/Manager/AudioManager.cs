using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    // [Header("Audio References")]
    // [SerializeField] private AudioSource musicAudioSource;
    // [SerializeField] private AudioSource sfxAudioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }


        // if (musicAudioSource == null || sfxAudioSource == null)
        // {
        //     Debug.LogError("AudioSource component not found in this GameObject.");
        // }
        // else
        // {
        //     musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        //     sfxAudioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.3f);
        // }
    }

}
