using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Audio References")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    private void Start()
    {
        // Mendaftarkan listener untuk scene load
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Jika AudioManager sudah ada sebelumnya, ambil komponen AudioSource
        TryGetAudioSources();
    }

    private void OnDestroy()
    {
        // Menghapus listener untuk mencegah memory leak
        SceneManager.sceneLoaded -= OnSceneLoaded;

        musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(UpdateSFXVolume);
    }

    // Fungsi yang akan dipanggil saat scene baru dimuat
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Coba untuk mendapatkan referensi AudioSource dari AudioManager
        TryGetAudioSources();
    }

    // Mencoba untuk mendapatkan referensi AudioSource dari AudioManager yang ada
    private void TryGetAudioSources()
    {
        // Cari AudioManager yang ada di scene
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            // Ambil AudioSource dari child MusicAudio
            musicAudioSource = audioManager.transform.Find("AudioMusic")?.GetComponent<AudioSource>();
            // Ambil AudioSource dari child SFXAudio
            sfxAudioSource = audioManager.transform.Find("AudioSFX")?.GetComponent<AudioSource>();

            Debug.Log($"TryGetAudioSources: musicAudio: {musicAudioSource} sfxAudio: {sfxAudioSource}");
            if (musicAudioSource != null)
            {
                musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
                musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
                musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
            }

            if (sfxAudioSource != null)
            {
                sfxAudioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.3f);
                sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.3f);
                sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
            }
        }
        else
        {
            Debug.LogError("AudioManager tidak ditemukan di scene!");
        }
    }


    // Fungsi untuk memperbarui volume AudioSource
    private void UpdateMusicVolume(float value)
    {
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
        }
    }

    private void UpdateSFXVolume(float value)
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = value;
            PlayerPrefs.SetFloat("SFXVolume", value);
            PlayerPrefs.Save();
        }
    }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class VolumeController : MonoBehaviour
// {
//     [Header("UI References")]
//     [SerializeField] private Slider musicVolumeSlider;
//     [SerializeField] private Slider sfxVolumeSlider;

//     [Header("Audio References")]
//     [SerializeField] private AudioSource musicAudioSource;
//     [SerializeField] private AudioSource sfxAudioSource;

//     private void Start()
//     {
//         musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
//         musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
//         musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
//         sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
//     }

//     // Fungsi untuk memperbarui volume AudioSource
//     private void UpdateMusicVolume(float value)
//     {
//         musicAudioSource.volume = value;
//         PlayerPrefs.SetFloat("MusicVolume", value);
//         PlayerPrefs.Save();
//     }

//     private void UpdateSFXVolume(float value)
//     {
//         sfxAudioSource.volume = value;
//         PlayerPrefs.SetFloat("SFXVolume", value);
//         PlayerPrefs.Save();
//     }

//     private void OnDestroy()
//     {
//         // Menghapus listener untuk mencegah memory leak
//         musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
//         sfxVolumeSlider.onValueChanged.RemoveListener(UpdateSFXVolume);
//     }
// }
