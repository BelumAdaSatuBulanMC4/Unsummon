using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogDisplay : MonoBehaviour
{
    public TextMeshProUGUI logText; // Assign via Inspector
    public ScrollRect scrollRect;   // Assign via Inspector (Drag your ScrollRect here)
    public GameObject showDebugView;
    public Button showDebug;

    private void Start()
    {
        showDebug.onClick.AddListener(ToggleDebugView);
    }

    private void ToggleDebugView()
    {
        bool isActive = showDebugView.activeSelf;
        showDebugView.SetActive(!isActive);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    // void HandleLog(string logString, string stackTrace, LogType type)
    // {
    //     logText.text += $"[!]=> {logString} \n";
    //     Canvas.ForceUpdateCanvases(); // Force the UI to update so the content height is recalculated
    //     ScrollToBottom();             // Scroll to the bottom after adding new log
    // }
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Warna berdasarkan jenis LogType
        string color;
        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                color = "red"; // Merah untuk Error
                break;
            case LogType.Warning:
                color = "yellow"; // Kuning untuk Warning
                break;
            default:
                color = "green"; // Putih untuk Log biasa
                break;
        }

        // Menambahkan log dengan tag warna TextMeshPro
        logText.text += $"<color={color}>[!]=> {logString}</color>\n";
        Canvas.ForceUpdateCanvases(); // Force the UI to update so the content height is recalculated
        ScrollToBottom();             // Scroll to the bottom after adding new log
    }

    void ScrollToBottom()
    {
        // ScrollRect moves between 0 (bottom) and 1 (top), so we set to 0 to move to the bottom
        scrollRect.verticalNormalizedPosition = 0f;
    }
}

