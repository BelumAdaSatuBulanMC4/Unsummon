using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_HomeSettings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUsername;
    [SerializeField] private Button editNameButton;
    [SerializeField] private GameObject UI_HomeSettingsObject;
    [SerializeField] private GameObject UI_EditNameObject;
    [SerializeField] private GameObject UI_Tutorial;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button helpButton;
    [SerializeField] private Button gameStroyButton;

    // Start is called before the first frame update
    void Start()
    {
        editNameButton.onClick.AddListener(() => UI_EditNameObject.SetActive(true));
        closeButton.onClick.AddListener(() => UI_HomeSettingsObject.SetActive(false));
        helpButton.onClick.AddListener(() => UI_Tutorial.SetActive(true));
        gameStroyButton.onClick.AddListener(() =>
        {
            Debug.Log("Start - harusnya pindah scene ke GameStory");
            SceneManager.LoadScene("GameStory");
        });
    }

    void Update()
    {
        textUsername.text = DataPersistence.LoadUsername();
    }

}
