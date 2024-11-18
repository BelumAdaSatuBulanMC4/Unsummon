using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{

    void Start()
    {
        // Cek apakah user pertama kali menggunakan game
        if (IsFirstTimeUser())
        {
            Debug.Log("Start - Sekarang user frist time ");
            // Jika pertama kali, pindah ke GameStory scene
            SceneManager.LoadScene("GameStory");
        }
        else
        {
            Debug.Log("Start - Sekarang user non frist time ");
        }
    }

    // Fungsi untuk mengecek apakah user pertama kali menggunakan game

    private bool IsFirstTimeUser()
    {
        // Cek apakah PlayerPrefs sudah ada untuk first-time key
        if (PlayerPrefs.HasKey("IsFirstTime"))
        {
            // Jika sudah, berarti ini bukan pertama kali
            return false;
        }
        else
        {
            // Jika tidak ada, berarti pertama kali dan tandai user sudah memainkan game
            PlayerPrefs.SetInt("IsFirstTime", 1);  // Menandai bahwa user sudah pertama kali menggunakan game
            PlayerPrefs.Save();  // Menyimpan perubahan
            return true;
        }
    }
}

