using TMPro;
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
        speechPlugin.Initialize();
        speechPlugin.StartGyro();
    }

    private void OnDestroy()
    {
        speechPlugin.StopRecording();
    }

    private void Update()
    {
        double roll = speechPlugin.GetRollValue();
        double pitch = speechPlugin.GetPitchValue();
        double yaw = speechPlugin.GetYawValue();

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

}

