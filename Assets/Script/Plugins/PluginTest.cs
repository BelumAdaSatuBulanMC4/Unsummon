using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PluginTest : MonoBehaviour
{

    private SwiftPlugin speechPlugin;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text info;
    [SerializeField] private Button button;

    private void Awake()
    {
        speechPlugin = GetComponent<SwiftPlugin>();
    }

    private void Start()
    {
        // Initialize the speech recognizer
        speechPlugin.Initialize();
        button.onClick.AddListener(speechPlugin.StartRecording);
    }

    private void Update()
    {
        text.text = "Transcribed Text: " + speechPlugin.GetTranscribedTextFromSwift();
        info.text = "Feedback Message: " + speechPlugin.GetFeedbackMessageFromSwift();
    }

    public void OnStartRecording()
    {
        speechPlugin.StartRecording();
    }

    public void OnStopRecording()
    {
        speechPlugin.StopRecording();
    }

    public void GetResults()
    {
        string transcribedText = speechPlugin.GetTranscribedTextFromSwift();
        Debug.Log("Transcribed Text: " + transcribedText);

        string feedbackMessage = speechPlugin.GetFeedbackMessageFromSwift();
        Debug.Log("Feedback Message: " + feedbackMessage);

        bool isRecording = speechPlugin.IsSwiftRecording();
        Debug.Log("Is Recording: " + isRecording);
    }

    // private SwiftPlugin swiftPlugin = new SwiftPlugin();
    // [SerializeField] TMP_Text text;

    // private void Start()
    // {
    //     swiftPlugin.Initialize();
    // }

    // private void Update()
    // {
    //     text.text = swiftPlugin.GetPrintSwift();
    // }

    // public void OnStartRecording()
    // {
    //     swiftPlugin.StartRecording();
    // }

    // public void OnStopRecording()
    // {
    //     swiftPlugin.StopRecording();
    // }
}
