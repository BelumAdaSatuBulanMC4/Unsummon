using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;  // Video Player yang akan menampilkan video ke Render Texture
    public RenderTexture renderTexture; // RenderTexture untuk video output
    public Button skipButton;  // Tombol Skip untuk melewatkan video
    public float videoDelay = 10f; // Waktu delay sebelum tombol Skip muncul

    private float videoTime = 0f;

    void Start()
    {
        // Memastikan tombol Skip dimulai dalam keadaan tidak aktif
        skipButton.gameObject.SetActive(false);

        // Menambahkan listener untuk tombol skip
        skipButton.onClick.AddListener(OnSkipButtonClicked);

        // Memulai video
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.Play();
    }

    void Update()
    {
        // Menambah waktu video yang sudah berjalan
        videoTime += Time.deltaTime;

        // Jika video sudah berjalan selama 10 detik, aktifkan tombol Skip
        if (videoTime >= videoDelay && !skipButton.gameObject.activeSelf)
        {
            skipButton.gameObject.SetActive(true);
        }
    }

    void OnSkipButtonClicked()
    {
        // Pindahkan ke MainMenu scene saat tombol skip diklik
        SceneManager.LoadScene("MainMenu");
    }
}
