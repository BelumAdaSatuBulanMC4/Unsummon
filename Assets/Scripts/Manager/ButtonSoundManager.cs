using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundManager : MonoBehaviour
{
    public AudioClip clickSound;  // Sound effect yang akan diputar
    public Button[] buttons;      // Array tombol yang akan diatur secara manual di Inspector
    private AudioSource audioSource;

    void Start()
    {
        // Mendapatkan AudioSource dari GameObject AudioManager
        audioSource = GetComponent<AudioSource>();

        // Menambahkan listener ke semua tombol yang telah diatur secara manual
        RegisterButtons();
    }

    // Mendaftarkan listener untuk setiap tombol yang diatur
    private void RegisterButtons()
    {
        foreach (Button btn in buttons)
        {
            if (btn != null)
            {
                btn.onClick.AddListener(() => PlayClickSound(btn));  // Tambahkan listener ke tombol
            }
        }
    }

    // Memutar sound effect ketika tombol diklik
    private void PlayClickSound(Button btn)
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);  // Memainkan sound effect ketika tombol diklik
        }
    }
}
