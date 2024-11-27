using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonSoundManager : MonoBehaviour
{
    private AudioSource audioSourceButton;
    public AudioClip clickSound;
    private Button[] allButtons;

    private void Awake()
    {
        // Memastikan bahwa FindObjectButton dipanggil pada scene pertama
        FindObjectButton();

        // Menambahkan listener untuk ketika scene baru dimuat
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        audioSourceButton = AudioManager.Instance.GetSFXAudioSource(0);
    }

    private void OnDestroy()
    {
        // Menghapus listener agar tidak menyebabkan memory leak
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Memanggil kembali FindObjectButton ketika scene baru dimuat
        FindObjectButton();
    }

    private void FindObjectButton()
    {
        allButtons = FindObjectsOfType<Button>(true);
        RegisterButtons();
    }

    // Mendaftarkan listener untuk setiap tombol yang diatur
    private void RegisterButtons()
    {
        foreach (Button btn in allButtons)
        {
            btn?.onClick.AddListener(PlayClickSound);  // Tambahkan listener ke tombol
        }
    }

    // Memutar sound effect ketika tombol diklik
    private void PlayClickSound()
    {
        if (audioSourceButton != null && clickSound != null)
        {
            audioSourceButton.PlayOneShot(clickSound);  // Memainkan sound effect ketika tombol diklik
            Debug.Log("PlayClickSound - berhasil dijalankan");
        }
    }
}




// using UnityEngine;
// using UnityEngine.UI;

// public class ButtonSoundManager : MonoBehaviour
// {
//     [SerializeField] private AudioSource audioSourceButton;
//     public AudioClip clickSound;
//     private Button[] allButtons;


//     private void Awake()
//     {
//         FindObjectButton();
//     }

//     private void FindObjectButton()
//     {
//         allButtons = FindObjectsOfType<Button>(true);
//         RegisterButtons();
//     }

//     // Mendaftarkan listener untuk setiap tombol yang diatur
//     private void RegisterButtons()
//     {
//         foreach (Button btn in allButtons)
//         {
//             btn?.onClick.AddListener(PlayClickSound);  // Tambahkan listener ke tombol
//         }
//     }

//     // Memutar sound effect ketika tombol diklik
//     private void PlayClickSound()
//     {
//         if (audioSourceButton != null && clickSound != null)
//         {
//             audioSourceButton.PlayOneShot(clickSound);  // Memainkan sound effect ketika tombol diklik
//         }
//     }
// }
