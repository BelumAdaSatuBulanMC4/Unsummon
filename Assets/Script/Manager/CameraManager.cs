using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("Camera Shake")]
    [SerializeField] private Vector2 shakeVelocity;

    private CinemachineImpulseSource source;

    [Header("Cinemachine Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;  // Reference to your Cinemachine camera

    private void Awake()
    {
        instance = this;
        source = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        // Cari karakter milik owner yang merupakan player di local client
        StartCoroutine(FindAuthorCharacterCoroutine());
    }

    // Coroutine untuk mencari karakter milik owner secara bertahap untuk menghindari race condition
    private IEnumerator FindAuthorCharacterCoroutine()
    {
        Character authorCharacter = null;
        while (authorCharacter == null)
        {
            // Mencari karakter milik pemain yang menjadi owner di local client
            authorCharacter = FindAuthorCharacter();
            if (authorCharacter != null)
            {
                // Jika karakter ditemukan, ubah kamera untuk mengikuti karakter tersebut
                ChangeCameraFollow(authorCharacter.transform);
            }
            else
            {
                Debug.LogWarning("No character with isAuthor found. Retrying...");
                yield return new WaitForSeconds(0.5f);  // Tunggu 0.5 detik sebelum mencoba lagi
            }
        }
    }

    public void CameraShake()
    {
        source.m_DefaultVelocity = new Vector2(shakeVelocity.x, shakeVelocity.y);
        source.GenerateImpulse();
    }

    // Method untuk mengubah target follow kamera Cinemachine
    public void ChangeCameraFollow(Transform newTarget)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = newTarget;
        }
    }

    // Menemukan karakter yang dimiliki oleh player local (isAuthor == true)
    private Character FindAuthorCharacter()
    {
        Character[] allCharacters = FindObjectsOfType<Character>();  // Menemukan semua karakter dalam scene
        foreach (Character character in allCharacters)
        {
            // Memastikan bahwa karakter tersebut adalah milik pemain di local client
            if (character.IsOwner)
            {
                return character;  // Mengembalikan karakter yang dimiliki oleh local client
            }
        }
        return null;  // Jika tidak ditemukan, kembalikan null
    }
}



// using System.Collections;
// using System.Collections.Generic;
// using Cinemachine;
// using Unity.Netcode;
// using UnityEngine;

// public class CameraManager : NetworkBehaviour
// {
//     public static CameraManager instance;

//     [Header("Camera Shake")]
//     [SerializeField] private Vector2 shakeVelocity;

//     private CinemachineImpulseSource source;

//     [Header("Cinemachine Camera")]
//     [SerializeField] private CinemachineVirtualCamera virtualCamera;  // Reference to your Cinemachine camera

//     private void Awake()
//     {
//         instance = this;
//         source = GetComponent<CinemachineImpulseSource>();
//     }

//     private void Start()
//     {
//         Character authorCharacter = FindAuthorCharacter();
//         if (authorCharacter != null)
//         {
//             ChangeCameraFollow(authorCharacter.transform);
//         }
//         else
//         {
//             Debug.LogWarning("No character with isAuthor found.");
//         }
//     }

//     public void CameraShake()
//     {
//         source.m_DefaultVelocity = new Vector2(shakeVelocity.x, shakeVelocity.y);
//         source.GenerateImpulse();
//     }

//     // Method to change the follow target of the Cinemachine camera
//     public void ChangeCameraFollow(Transform newTarget)
//     {
//         if (virtualCamera != null)
//         {
//             virtualCamera.Follow = newTarget;
//         }
//     }

//     private Character FindAuthorCharacter()
//     {
//         Character[] allCharacters = FindObjectsOfType<Character>();  // Find all characters in the scene
//         foreach (Character character in allCharacters)
//         {
//             if (character.isAuthor)
//             {
//                 return character;  // Return the first character with isAuthor set to true
//             }
//         }
//         return null;  // Return null if no author character is found
//     }
// }
