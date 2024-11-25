using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(UpdateSFXVolume);
    }

    // Fungsi untuk memperbarui volume AudioSource
    private void UpdateMusicVolume(float value)
    {
        musicAudioSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    private void UpdateSFXVolume(float value)
    {
        sfxAudioSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        // Menghapus listener untuk mencegah memory leak
        musicVolumeSlider.onValueChanged.RemoveListener(UpdateMusicVolume);
        sfxVolumeSlider.onValueChanged.RemoveListener(UpdateSFXVolume);
    }
}
