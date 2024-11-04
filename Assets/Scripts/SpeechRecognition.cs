using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechRecognition : MonoBehaviour
{
    private SwiftPlugin swiftPlugin;
    void Start()
    {
        swiftPlugin = GetComponent<SwiftPlugin>();
        swiftPlugin.Initialize();
        StartCoroutine(StartAndStopRecordingIn(5f));
        string text = swiftPlugin.GetTranscribedTextFromSwift();
        Debug.Log($"Kamu bilang: {text}");
    }

    private IEnumerator StartAndStopRecordingIn(float delay)
    {
        swiftPlugin.StartRecording();
        yield return new WaitForSeconds(delay);
        swiftPlugin.StopRecording();
    }


}
