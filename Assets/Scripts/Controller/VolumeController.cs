using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class VolumeController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [Header("Audio References")]
    [SerializeField] private AudioSource musicAudioSource;
    private AudioSource[] sfxAudioSources;

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
        TryGetAudioSources();
    }

    // Mencoba untuk mendapatkan referensi AudioSource dari AudioManager yang ada
    private void TryGetAudioSources()
    {
        musicAudioSource = AudioManager.Instance.GetMusicAudioSource();
        sfxAudioSources = AudioManager.Instance.GetAllSFXAudioSource();
        // Cari AudioManager yang ada di scene
        Debug.Log($"TryGetAudioSources: musicAudio: {musicAudioSource} sfxAudio: {sfxAudioSources.Length}");
        if (musicAudioSource != null)
        {
            musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        }
        else Debug.Log("TryGetAudioSources - musicAudioSource not found");

        if (sfxAudioSources.Length > 0 && sfxAudioSources != null)
        {
            for (int i = 0; i < sfxAudioSources.Length; i++)
            {
                Debug.Log($"TryGetAudioSources: Volume {i}: {sfxAudioSources[i].volume}");
                sfxAudioSources[i].volume = PlayerPrefs.GetFloat("SFXVolume", 0.3f);
            }
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.3f);
            sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
        }
        else Debug.Log("TryGetAudioSources - sfxAudioSources not found");

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
        if (sfxAudioSources.Length > 0)
        {
            for (int i = 0; i < sfxAudioSources.Length; i++)
            {
                sfxAudioSources[i].volume = value;
            }
            PlayerPrefs.SetFloat("SFXVolume", value);
            PlayerPrefs.Save();
        }
    }
}


