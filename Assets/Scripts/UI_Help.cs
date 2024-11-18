using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Help : MonoBehaviour
{
    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private GameObject[] tutorialView;

    private int currentContent = 0;

    private void Start()
    {
        rightButton.onClick.AddListener(ChangeNextContent);
        leftButton.onClick.AddListener(ChangePreviousContent);
    }

    private void ChangeNextContent()
    {
        Debug.Log($"ChangeNextContent: currentContent: {currentContent}");
        Debug.Log($"ChangeNextContent: length: {tutorialView.Length}");
        if (currentContent >= (tutorialView.Length - 1)) return;
        tutorialView[currentContent].SetActive(false);
        currentContent++;
        tutorialView[currentContent].SetActive(true);
        Debug.Log($"ChangeNextContent: currentContent after increase: {currentContent}");
    }

    private void ChangePreviousContent()
    {
        if (currentContent <= 0) return;
        tutorialView[currentContent].SetActive(false);
        currentContent--;
        tutorialView[currentContent].SetActive(true);
    }


}
