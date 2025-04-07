using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

// Digunakan pada GameObject VideoControl
// Terdapat di Scene GameStory
public class VideoControl : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;  // Komponen VideoPlayer yang digunakan untuk menampilkan video
    public RenderTexture renderTexture; // RenderTexture yang digunakan sebagai output video
    public Button skipButton;  // Tombol Skip yang memungkinkan pemain melewati video
    public float videoDelay = 10f; // Waktu (dalam detik) sebelum tombol Skip muncul
    private float videoTime = 0f; // Variabel untuk melacak waktu pemutaran video

    void Start()
    {
        // Menonaktifkan tombol Skip saat pertama kali scene dimulai
        skipButton.gameObject.SetActive(false);

        // Menambahkan event listener ke tombol Skip agar menjalankan fungsi saat diklik
        skipButton.onClick.AddListener(OnSkipButtonClicked);

        // Mengatur target output video ke RenderTexture yang telah ditentukan
        videoPlayer.targetTexture = renderTexture;

        // Memulai pemutaran video
        videoPlayer.Play();
    }

    void Update()
    {
        // Menambah waktu pemutaran video sesuai dengan deltaTime
        videoTime += Time.deltaTime;

        // Jika video sudah berjalan selama waktu delay yang ditentukan, aktifkan tombol Skip
        if (videoTime >= videoDelay && !skipButton.gameObject.activeSelf)
        {
            skipButton.gameObject.SetActive(true);
        }
    }

    void OnSkipButtonClicked()
    {
        // Pindah ke scene "MainMenu" ketika tombol Skip diklik
        SceneManager.LoadScene("MainMenu");
    }
}
