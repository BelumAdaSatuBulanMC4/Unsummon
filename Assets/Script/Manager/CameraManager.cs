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

    private void Awake()
    {
        instance = this;
        source = GetComponent<CinemachineImpulseSource>();
    }

    public void CameraShake()
    {
        source.m_DefaultVelocity = new Vector2(shakeVelocity.x, shakeVelocity.y);
        source.GenerateImpulse();
    }
}
