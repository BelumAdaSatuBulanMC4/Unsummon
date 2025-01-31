using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Tambahkan namespace ini untuk EventTrigger

public class ProgressBarController : MonoBehaviour
{
    [SerializeField] private Button candleButton;
    public Image progressBar; // Referensi ke Image Progress Bar
    private float fillTime = 3f; // Waktu yang dibutuhkan untuk mengisi progress bar
    private float currentFill = 0f; // Nilai saat ini dari progress bar
    private bool isFilling = false; // Status pengisian progress bar
    private float holdDuration = 0f; // Waktu layar ditekan

    private void Start()
    {
        // Menambahkan EventTrigger untuk button
        EventTrigger trigger = candleButton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { StartFillingProgressBar(); });
        trigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { StopFillingProgressBar(); });
        trigger.triggers.Add(pointerUpEntry);
    }

    private void Update()
    {
        if (isFilling) // Jika sedang mengisi
        {
            holdDuration += Time.deltaTime; // Tambahkan waktu penahanan

            // Jika waktu penahanan mencapai fillTime (3 detik)
            if (holdDuration >= fillTime)
            {
                currentFill = 1f; // Set fill ke maksimum
                progressBar.fillAmount = currentFill; // Update nilai progress bar
                StopFillingProgressBar(); // Hentikan pengisian setelah penuh
            }
            else
            {
                currentFill = holdDuration / fillTime; // Hitung nilai saat ini
                progressBar.fillAmount = currentFill; // Update nilai progress bar
            }
        }
    }

    // Memulai pengisian progress bar
    public void StartFillingProgressBar()
    {
        isFilling = true; // Mulai mengisi progress bar
        holdDuration = 0f; // Reset waktu penahanan
    }

    // Menghentikan pengisian progress bar
    public void StopFillingProgressBar()
    {
        isFilling = false; // Hentikan pengisian
        StartCoroutine(ResetProgressBar()); // Mulai reset progress bar jika belum penuh
    }

    private System.Collections.IEnumerator ResetProgressBar()
    {
        while (currentFill > 0) // Reset progress bar
        {
            currentFill -= Time.deltaTime / 1f; // Turunkan nilai
            currentFill = Mathf.Clamp01(currentFill); // Batasi nilai
            progressBar.fillAmount = currentFill; // Update nilai progress bar
            yield return null; // Tunggu sampai frame berikutnya
        }
    }
}



// using UnityEngine;
// using UnityEngine.UI;

// public class ProgressBarController : MonoBehaviour
// {
//     [SerializeField] Button candleButton;
//     public Image progressBar; // Referensi ke Image Progress Bar
//     private float fillTime = 3f; // Waktu yang dibutuhkan untuk mengisi progress bar
//     private float currentFill = 0f; // Nilai saat ini dari progress bar
//     private bool isFilling = false; // Status pengisian progress bar
//     private float holdDuration = 0f; // Waktu layar ditekan

//     void Update()
//     {
//         // Memeriksa input dari layar
//         if (Input.GetMouseButtonDown(0)) // Jika layar ditekan
//         {
//             isFilling = true; // Mulai mengisi progress bar
//             holdDuration = 0f; // Reset waktu penahanan
//         }

//         if (Input.GetMouseButton(0) && isFilling) // Jika layar masih ditekan
//         {
//             holdDuration += Time.deltaTime; // Tambahkan waktu penahanan

//             // Jika waktu penahanan mencapai fillTime (3 detik)
//             if (holdDuration >= fillTime)
//             {
//                 currentFill = 1f; // Set fill ke maksimum
//                 progressBar.fillAmount = currentFill; // Update nilai progress bar
//             }
//             else
//             {
//                 currentFill = holdDuration / fillTime; // Hitung nilai saat ini
//                 progressBar.fillAmount = currentFill; // Update nilai progress bar
//             }
//         }

//         if (Input.GetMouseButtonUp(0)) // Jika layar dilepaskan
//         {
//             // Jika sudah terisi penuh, tidak perlu melakukan apa-apa
//             if (currentFill >= 1f)
//             {
//                 Debug.Log("Progress bar penuh!");
//                 return; // Keluar dari fungsi tanpa mereset progress bar
//             }

//             // Hentikan pengisian jika belum penuh
//             isFilling = false;
//             StartCoroutine(ResetProgressBar()); // Mulai reset progress bar jika belum penuh
//         }
//     }

//     private System.Collections.IEnumerator ResetProgressBar()
//     {
//         while (currentFill > 0) // Reset progress bar
//         {
//             currentFill -= Time.deltaTime / 1f; // Turunkan nilai
//             currentFill = Mathf.Clamp01(currentFill); // Batasi nilai
//             progressBar.fillAmount = currentFill; // Update nilai progress bar
//             yield return null; // Tunggu sampai frame berikutnya
//         }
//     }
// }