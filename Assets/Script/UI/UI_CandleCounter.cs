using TMPro;
using UnityEngine;

public class UI_CandleCounter : MonoBehaviour
{
    private TextMeshProUGUI candleText;
    [SerializeField] GameObject[] progressBar;
    [SerializeField] GameObject[] progressBarCurse;


    private void Awake()
    {
        candleText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        UpdateCandleCounter(GameManager.instance.GetActiveItems());
    }

    private void Update()
    {
        UpdateCandleCounter(GameManager.instance.GetActiveItems());
    }

    public void UpdateCandleCounter(int activeItems)
    {
        for (int i = 0; i < progressBar.Length; i++)
        {
            progressBar[i].SetActive(i < activeItems);
        }
    }
}
