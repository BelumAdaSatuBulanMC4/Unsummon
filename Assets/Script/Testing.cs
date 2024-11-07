using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Haptic : MonoBehaviour
{
    SwiftPlugin swiftPlugin;
    [SerializeField] private Button second1;
    [SerializeField] private Button second2;
    [SerializeField] private Button second3;
    [SerializeField] private Button second4;
    [SerializeField] private Button second5;

    private void Awake()
    {
        swiftPlugin = GetComponent<SwiftPlugin>();
    }

    private void Start()
    {
        second1.onClick.AddListener(FirstHaptic);
        second2.onClick.AddListener(SecondHaptic);
        second3.onClick.AddListener(ThirdHaptic);
        second4.onClick.AddListener(FourthHaptic);
        second5.onClick.AddListener(FifthHaptic);
    }

    private void FirstHaptic()
    {
        swiftPlugin.TriggerHapticFeedback(.7f, 1);
    }
    private void SecondHaptic()
    {
        swiftPlugin.TriggerHapticFeedback(.7f, 1);
    }
    private void ThirdHaptic()
    {
        swiftPlugin.TriggerHapticFeedback(.7f, 1);
    }
    private void FourthHaptic()
    {
        swiftPlugin.TriggerHapticFeedback(.7f, 1);
    }
    private void FifthHaptic()
    {
        swiftPlugin.TriggerHapticFeedback(.7f, 1);
    }
}
