// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class IndicatorManager : MonoBehaviour
// {
//     // UI indicator prefabs for Pocong, Kid, and Spirit
//     public RectTransform pocongIndicatorPrefab;
//     public RectTransform kidIndicatorPrefab;
//     public RectTransform spiritIndicatorPrefab;

//     // Reference to the camera
//     public Camera mainCamera;

//     // Lists to store active indicators
//     private RectTransform pocongIndicator;
//     private Dictionary<PlayerKid, RectTransform> kidIndicators = new Dictionary<PlayerKid, RectTransform>();
//     private Dictionary<PlayerSpirit, RectTransform> spiritIndicators = new Dictionary<PlayerSpirit, RectTransform>();

//     private void Update()
//     {
//         Debug.Log("di dalem indicator manager");
//         // Update Pocong indicator if Pocong exists
//         UpdatePocongIndicator();

//         // Update Kid indicators if Kids exist
//         foreach (PlayerKid kid in PlayerManager.instance.GetAllKids())
//         {
//             if (kidIndicators.ContainsKey(kid))
//             {
//                 UpdateKidIndicator(kid, kid.transform.position);
//             }
//             else
//             {
//                 // Create a new indicator for a new Kid
//                 CreateKidIndicator(kid);
//             }
//         }

//         // Update Spirit indicators if Spirits exist
//         foreach (var spiritEntry in PlayerManager.instance.GetSpiritPositions())
//         {
//             PlayerSpirit spirit = spiritEntry.Key;
//             if (spiritIndicators.ContainsKey(spirit))
//             {
//                 UpdateSpiritIndicator(spirit, spirit.transform.position);
//             }
//             else
//             {
//                 // Create a new indicator for a new Spirit
//                 CreateSpiritIndicator(spirit);
//             }
//         }
//     }

//     // Create and update Pocong's indicator
//     private void UpdatePocongIndicator()
//     {
//         Vector3 pocongPosition = PlayerManager.instance.getPocongPosition();

//         if (pocongPosition == Vector3.zero)
//         {
//             if (pocongIndicator != null)
//             {
//                 pocongIndicator.gameObject.SetActive(false);
//             }
//             return;
//         }

//         if (pocongIndicator == null)
//         {
//             pocongIndicator = Instantiate(pocongIndicatorPrefab, mainCamera.transform);
//         }

//         Vector3 screenPos = mainCamera.WorldToScreenPoint(pocongPosition);

//         if (screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
//         {
//             screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
//             screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);
//         }

//         pocongIndicator.position = screenPos;
//         pocongIndicator.gameObject.SetActive(true);
//     }

//     // Create and update Kid's indicator
//     private void CreateKidIndicator(PlayerKid kid)
//     {
//         RectTransform kidIndicator = Instantiate(kidIndicatorPrefab, mainCamera.transform);
//         kidIndicators.Add(kid, kidIndicator);
//         UpdateKidIndicator(kid, kid.transform.position);
//     }

//     private void UpdateKidIndicator(PlayerKid kid, Vector3 kidPosition)
//     {
//         Debug.Log("di sini");
//         RectTransform kidIndicator = kidIndicators[kid];

//         Vector3 screenPos = mainCamera.WorldToScreenPoint(kidPosition);

//         if (screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
//         {
//             screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
//             screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);
//         }

//         kidIndicator.position = screenPos;
//         kidIndicator.gameObject.SetActive(true);
//     }

//     // Create and update Spirit's indicator
//     private void CreateSpiritIndicator(PlayerSpirit spirit)
//     {
//         RectTransform spiritIndicator = Instantiate(spiritIndicatorPrefab, mainCamera.transform);
//         spiritIndicators.Add(spirit, spiritIndicator);
//         UpdateSpiritIndicator(spirit, spirit.transform.position);
//     }

//     private void UpdateSpiritIndicator(PlayerSpirit spirit, Vector3 spiritPosition)
//     {
//         RectTransform spiritIndicator = spiritIndicators[spirit];

//         Vector3 screenPos = mainCamera.WorldToScreenPoint(spiritPosition);

//         if (screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
//         {
//             screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
//             screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);
//         }

//         spiritIndicator.position = screenPos;
//         spiritIndicator.gameObject.SetActive(true);
//     }
// }
