using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class SwiftPlugin : MonoBehaviour
{

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
    private bool isRecordingStarted = false;

    // Call the Initialize function
    public void Initialize()
    {
        InitializeSpeechRecognizer();
        Debug.Log("Speech recognizer initialized");
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

    // Declare the Swift functions exposed via DllImport
    // [DllImport("__Internal")]
    // private static extern void InitializeSpeechRecognizer();

    // [DllImport("__Internal")]
    // private static extern void StartSpeechRecognition();

    // [DllImport("__Internal")]
    // private static extern void StopSpeechRecognition();

    // [DllImport("__Internal")]
    // private static extern System.IntPtr printSomething();

    // Call the Swift functions from Unity
    // [SerializeField] TMP_Text text;




    // [DllImport("__Internal")]
    // private static extern void InitializeSpeechRecognizer();

    // [DllImport("__Internal")]
    // private static extern void StartSpeechRecognition();

    // [DllImport("__Internal")]
    // private static extern void StopSpeechRecognition();

    // [DllImport("__Internal")]
    // private static extern IntPtr GetRecognitionResult();

    // public bool isCorrect = false;

    // private void Start()
    // {
    //     InitializeSpeechRecognizer();  // Initialize the recognizer
    // }

    // public void StartRecognition()
    // {
    //     StartSpeechRecognition();
    // }

    // public void StopRecognition()
    // {
    //     StopSpeechRecognition();
    // }

    // private void Update()
    // {
    //     // Get the recognition result from the plugin
    //     string result = Marshal.PtrToStringAuto(GetRecognitionResult());

    //     if (result == "true")
    //     {
    //         isCorrect = true;
    //         text.text = "Correct word recognized.";
    //         Debug.Log("Correct word recognized.");
    //     }
    //     else if (result == "false")
    //     {
    //         isCorrect = false;
    //         text.text = "Incorrect word recognized.";
    //         Debug.Log("Incorrect word recognized.");
    //     }
    // }




    // public void Initialize()
    // {
    //     InitializeSpeechRecognizer();
    //     Debug.Log("Speech recognizer initialized in Swift");
    // }

    // public void StartRecording()
    // {
    //     StartSpeechRecognition();
    //     Debug.Log("Speech recognition started in Swift");
    // }

    // public void StopRecording()
    // {
    //     StopSpeechRecognition();
    //     Debug.Log("Speech recognition stopped in Swift");
    // }

    // public string GetPrintSwift()
    // {
    //     // Convert the pointer returned by the Swift function to a C# string
    //     return Marshal.PtrToStringAuto(printSomething());
    // }
}
