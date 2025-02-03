using TMPro;
using UnityEngine;

public class UI_RoomName : MonoBehaviour
{
    private Character chara;
    private TextMeshProUGUI locationText;
    // Start is called before the first frame update

    private void Awake()
    {
        locationText = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        chara = FindAuthorCharacter();
        if (chara != null)
        {
            locationText.text = chara.GetCurrentLocation();
        }
        else
        {
            Debug.LogWarning("No player with isAuthor found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (chara != null)
        {
            locationText.text = chara.GetCurrentLocation();
        }
    }

    private Character FindAuthorCharacter()
    {
        // Find all Character objects (this will find all PlayerKid, Pocong, and PlayerSpirit)
        Character[] allCharacters = FindObjectsOfType<Character>();

        foreach (Character character in allCharacters)
        {
            if (character.isAuthor)
            {
                return character;
            }
        }

        return null;
    }
}
