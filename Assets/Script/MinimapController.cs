using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    public RectTransform minimapBackground;
    public RectTransform objectIndicator;
    //objectToTrack udah ga perlu ya, bisa dihapus aja karena udh pake authorcharacter
    // public Transform objectToTrack;
    public Vector2 mapSize = new Vector2(735, 575);
    public Vector2 worldBounds = new Vector2(100, 100);

    private List<Vector3> candles = new List<Vector3>();
    private List<Vector3> cursed = new List<Vector3>();

    [Header("indicator")]
    [SerializeField] private GameObject kidIndicator;
    [SerializeField] private GameObject pocongIndicator;
    [SerializeField] private GameObject candleIndicator;
    [SerializeField] private GameObject mirrorIndicator;
    [SerializeField] private GameObject closetIndicator;


    void Update()
    {
        // Vector3 objectPosition = objectToTrack.position;
        Vector3 charPosition = UI_InGame.instance.GetAuthorCharacterPosition();
        candles = UI_InGame.instance.GetCandleRegistered();
        Vector2 normalizedPosition = new Vector2(Mathf.InverseLerp(-worldBounds.x, worldBounds.x, charPosition.x), Mathf.InverseLerp(-worldBounds.y, worldBounds.y, charPosition.y));

        Vector2 minimapPosition = new Vector2(
            normalizedPosition.x * mapSize.x - (mapSize.x / 2),
            normalizedPosition.y * mapSize.y - (mapSize.y / 2)
        );

        objectIndicator.anchoredPosition = minimapPosition;

        // if (UI_InGame.instance.GetAuthorCharacterType() == "Player")
        // {
        //     Vector2 normalizedPosition = new Vector2(
        //         Mathf.InverseLerp(-worldBounds.x, worldBounds.x, charPosition.x),
        //         Mathf.InverseLerp(-worldBounds.y, worldBounds.y, charPosition.y)
        //     );

        //     Vector2 minimapPosition = new Vector2(
        //         normalizedPosition.x * mapSize.x - (mapSize.x / 2),
        //         normalizedPosition.y * mapSize.y - (mapSize.y / 2)
        //     );

        //     objectIndicator.anchoredPosition = minimapPosition;
        // }
        // else
        // {
        //     Vector2 normalizedPosition = new Vector2(
        //         Mathf.InverseLerp(-worldBounds.x, worldBounds.x, charPosition.x),
        //         Mathf.InverseLerp(-worldBounds.y, worldBounds.y, charPosition.y)
        //     );

        //     Vector2 minimapPosition = new Vector2(
        //         normalizedPosition.x * mapSize.x - (mapSize.x / 2),
        //         normalizedPosition.y * mapSize.y - (mapSize.y / 2)
        //     );

        //     objectIndicator.anchoredPosition = minimapPosition;
        // }
    }
}
