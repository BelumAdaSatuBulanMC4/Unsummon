// using System.Runtime.InteropServices;
// using UnityEngine;
// using UnityEngine.UI;

// public class GyroControlledSlider : MonoBehaviour
// {
//     [DllImport("__Internal")]
//     private static extern double GetPitch();  // Import pitch for slider movement

//     public RectTransform handle;        // Assign the handle in the Inspector
//     public RectTransform sliderBackground;  // Assign the background in the Inspector

//     public float sensitivity = 5.0f;    // Sensitivity of gyro input
//     private float minPositionX;         // Minimum X position of the handle
//     private float maxPositionX;         // Maximum X position of the handle

//     private void Start()
//     {
//         // Initialize minimum and maximum positions for the handle based on background
//         minPositionX = sliderBackground.rect.xMin;
//         maxPositionX = sliderBackground.rect.xMax;

//         // Start gyro updates in Swift (iOS only)
// #if UNITY_IOS && !UNITY_EDITOR
//         StartGyroUpdates();
// #endif
//     }

//     private void OnDestroy()
//     {
//         // Stop gyro updates in Swift (iOS only)
// #if UNITY_IOS && !UNITY_EDITOR
//         StopGyroUpdates();
// #endif
//     }

//     private void Update()
//     {
// #if UNITY_IOS && !UNITY_EDITOR
//         // Get pitch from gyroscope
//         double pitch = GetPitch();

//         // Calculate the handle's new position based on pitch, sensitivity, and min/max bounds
//         float targetPositionX = Mathf.Clamp((float)pitch * sensitivity, minPositionX, maxPositionX);

//         // Update handle position within slider background
//         handle.anchoredPosition = new Vector2(targetPositionX, handle.anchoredPosition.y);
// #endif
//     }

//     public float GetSliderValue()
//     {
//         // Calculate slider value between 0 and 1 based on handle position
//         return Mathf.InverseLerp(minPositionX, maxPositionX, handle.anchoredPosition.x);
//     }
// }
