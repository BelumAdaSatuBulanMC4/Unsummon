using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    private const string FirstTimeKey = "FirstTimeUser"; // Key untuk menyimpan status first-time user

    void Start()
    {
        // Cek apakah user pertama kali menggunakan game
        if (IsFirstTimeUser())
        {
            // Jika pertama kali, pindah ke GameStory scene
            SceneManager.LoadScene("GameStory");
        }
        else
        {
            // Jika bukan pertama kali, pindah ke MainMenu scene
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Fungsi untuk mengecek apakah user pertama kali menggunakan game
    private bool IsFirstTimeUser()
    {
        // Cek apakah PlayerPrefs sudah ada untuk first-time key
        if (PlayerPrefs.HasKey(FirstTimeKey))
        {
            // Jika sudah, berarti ini bukan pertama kali
            return false;
        }
        else
        {
            // Jika tidak ada, berarti pertama kali dan tandai user sudah memainkan game
            PlayerPrefs.SetInt(FirstTimeKey, 1);  // Menandai bahwa user sudah pertama kali menggunakan game
            PlayerPrefs.Save();  // Menyimpan perubahan
            return true;
        }
    }
}

