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
        speechPlugin.StartGyro();
        // button.onClick.AddListener(speechPlugin.StartRecording);
    }

    private void OnDestroy()
    {
        speechPlugin.StopRecording();
    }

    // private void Update()
    // {
    //     text.text = "Transcribed Text: " + speechPlugin.GetTranscribedTextFromSwift();
    //     info.text = "Feedback Message: " + speechPlugin.GetFeedbackMessageFromSwift();
    // }

    private void Update()
    {
        // Fetch roll, pitch, and yaw from CoreMotionManager
        double roll = speechPlugin.GetRollValue();
        double pitch = speechPlugin.GetPitchValue();
        double yaw = speechPlugin.GetYawValue();

        // Use pitch and roll to move character based on tilt
        Vector3 movement = new Vector3((float)roll, 0, (float)pitch) * 3;
        transform.Translate(movement * Time.deltaTime, Space.World);
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
