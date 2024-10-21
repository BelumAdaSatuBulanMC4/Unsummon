using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SwiftPlugin : MonoBehaviour
{
    // Declare the Swift functions exposed via DllImport
    [DllImport("__Internal")]
    private static extern void InitializeSpeechRecognizer();

    [DllImport("__Internal")]
    private static extern void StartSpeechRecognition();

    [DllImport("__Internal")]
    private static extern void StopSpeechRecognition();

    [DllImport("__Internal")]
    private static extern System.IntPtr printSomething();

    // Call the Swift functions from Unity
    public void Initialize()
    {
        InitializeSpeechRecognizer();
        Debug.Log("Speech recognizer initialized in Swift");
    }

    public void StartRecording()
    {
        StartSpeechRecognition();
        Debug.Log("Speech recognition started in Swift");
    }

    public void StopRecording()
    {
        StopSpeechRecognition();
        Debug.Log("Speech recognition stopped in Swift");
    }

    public string GetPrintSwift()
    {
        // Convert the pointer returned by the Swift function to a C# string
        return Marshal.PtrToStringAuto(printSomething());
    }
}
