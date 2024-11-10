using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class SwiftPlugin : MonoBehaviour
{
    //SPEECH
    [DllImport("__Internal")]
    private static extern void InitializeSpeechRecognizer();

    [DllImport("__Internal")]
    private static extern void StartSpeechRecognition();

    [DllImport("__Internal")]
    private static extern void StopSpeechRecognition();

    [DllImport("__Internal")]
    private static extern IntPtr GetTranscribedText();

    [DllImport("__Internal")]
    private static extern bool IsRecording();

    [DllImport("__Internal")]
    private static extern IntPtr GetFeedbackMessage();

    //================================================================
    //COREMOTION
    [DllImport("__Internal")]
    private static extern double GetRoll();

    [DllImport("__Internal")]
    private static extern double GetPitch();

    [DllImport("__Internal")]
    private static extern double GetYaw();

    [DllImport("__Internal")]
    private static extern void StartGyroUpdates();

    [DllImport("__Internal")]
    private static extern void StopGyroUpdates();

    //================================================================
    // HAPTIC FEEDBACK
    [DllImport("__Internal")]
    private static extern void PlayHaptic(float intensity, float duration);

    [DllImport("__Internal")]
    private static extern void StopHaptic();

    [DllImport("__Internal")]
    private static extern void StartContinuousHaptic(float intensity);

    [DllImport("__Internal")]
    private static extern void StopContinuousHaptic();

    private bool isRecordingStarted = false;

    // Call the Initialize function
    public void Initialize()
    {
        InitializeSpeechRecognizer();
        Debug.Log("Speech recognizer initialized");
    }

    public void StartGyro()
    {
        StartGyroUpdates();
    }

    public void StopGyro()
    {
        StopGyroUpdates();
    }

    // Call the StartSpeechRecognition function
    public void StartRecording()
    {
        StartSpeechRecognition();

        // if (!isRecordingStarted)
        // {
        //     StartSpeechRecognition();
        //     Debug.Log("Started recording");
        //     isRecordingStarted = true;
        // }
    }

    // Call the StopSpeechRecognition function
    public void StopRecording()
    {
        StopSpeechRecognition();

        // if (isRecordingStarted)
        // {
        //     StopSpeechRecognition();
        //     Debug.Log("Stopped recording");
        //     isRecordingStarted = false;
        // }
    }

    // Call the GetTranscribedText function
    public string GetTranscribedTextFromSwift()
    {
        IntPtr ptr = GetTranscribedText();
        return Marshal.PtrToStringAuto(ptr);
    }

    // Call the IsRecording function
    public bool IsSwiftRecording()
    {
        return IsRecording();
    }

    // Call the GetFeedbackMessage function
    public string GetFeedbackMessageFromSwift()
    {
        IntPtr ptr = GetFeedbackMessage();
        return Marshal.PtrToStringAuto(ptr);
    }

    public double GetRollValue()
    {
        return GetRoll();
    }

    public double GetPitchValue()
    {
        return GetPitch();
    }

    public double GetYawValue()
    {
        return GetYaw();
    }

    // HAPTIC!
    public void TriggerHapticFeedback(float intensity, float duration)
    {
        PlayHaptic(intensity, duration);
    }

    public void StartConHapticFeedback(float intensity)
    {
        StartContinuousHaptic(intensity);
    }

    public void StopConHapticFeedback()
    {
        StopContinuousHaptic();
    }

    public void StopHapticFeedback()
    {
        StopHaptic();
    }

}
