using UnityEngine;
using System.Collections;

public class MicrophoneRecorder : MonoBehaviour
{
    private AudioClip recordedClip;

    void Start()
    {
        StartCoroutine(RequestMicPermissionAndRecord());
    }

    private IEnumerator RequestMicPermissionAndRecord()
    {
        // Meminta izin untuk menggunakan mikrofon
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

        // Memeriksa apakah izin diberikan
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("Mikrofon diizinkan, mulai merekam...");

            // Merekam suara selama 5 detik
            recordedClip = Microphone.Start(null, false, 5, 44100);
            yield return new WaitForSeconds(5); // Tunggu selama 5 detik

            // Menghentikan perekaman
            Microphone.End(null);
            Debug.Log("Rekaman selesai.");

            // Lakukan sesuatu dengan recordedClip (misalnya, simpan atau analisis)
        }
        else
        {
            Debug.Log("Izin mikrofon ditolak.");
            // Tampilkan pesan kepada pengguna atau lakukan penanganan lainnya
        }
    }
}
