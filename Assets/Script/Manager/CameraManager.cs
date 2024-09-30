using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

    private void Start() {
        Character authorCharacter = FindAuthorCharacter();
        if (authorCharacter != null)
        {
            ChangeCameraFollow(authorCharacter.transform);
        }
        else
        {
            Debug.LogWarning("No character with isAuthor found.");
        }
    }

    public void CameraShake()
    {
        source.m_DefaultVelocity = new Vector2(shakeVelocity.x, shakeVelocity.y);
        source.GenerateImpulse();
    }

    // Method to change the follow target of the Cinemachine camera
    public void ChangeCameraFollow(Transform newTarget)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = newTarget;
        }
    }

    private Character FindAuthorCharacter()
    {
        Character[] allCharacters = FindObjectsOfType<Character>();  // Find all characters in the scene
        foreach (Character character in allCharacters)
        {
            if (character.isAuthor)
            {
                return character;  // Return the first character with isAuthor set to true
            }
        }
        return null;  // Return null if no author character is found
    }
}
