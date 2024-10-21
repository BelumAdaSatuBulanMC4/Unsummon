using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PluginTest : MonoBehaviour
{
    private SwiftPlugin swiftPlugin = new SwiftPlugin();
    [SerializeField] TMP_Text text;

    private void Start()
    {
        swiftPlugin.Initialize();
    }

    private void Update()
    {
        text.text = swiftPlugin.GetPrintSwift();
    }

    public void OnStartRecording()
    {
        swiftPlugin.StartRecording();
    }

    public void OnStopRecording()
    {
        swiftPlugin.StopRecording();
    }
}
