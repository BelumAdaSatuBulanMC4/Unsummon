// using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class JoystickGame : MonoBehaviour
{
    [SerializeField] private RectTransform joystickBase;
    [SerializeField] private RectTransform joystickKnob;
    [SerializeField] private RectTransform onScreenStick;
    // [SerializeField] private TMP_Text tulisan;
    // private Vector2 joystickPosition = Vector2.zero;
    private bool isTouching = false;

    private Vector2 positionKnob;
    private Vector2 positionBase;
    private Vector2 positionStick;


    private void Start()
    {
        positionBase = joystickBase.localPosition;
        positionKnob = joystickKnob.localPosition;
    }

    void Update()
    {
        // tulisan.text = "x: " + GetJoystickDirection().x + " dan y:" + GetJoystickDirection().y;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);


            if (touch.position.x < Screen.width / 2)
            {
                if (!isTouching)
                {
                    isTouching = true;
                    joystickBase.gameObject.SetActive(true);
                    joystickKnob.gameObject.SetActive(true);
                    joystickBase.position = touch.position;
                    joystickKnob.position = touch.position;
                    onScreenStick.localPosition = touch.position;
                    Debug.Log("position: " + touch.position);
                }

                Vector2 touchLocalPosition = (Vector2)joystickBase.InverseTransformPoint(touch.position);
                onScreenStick.localPosition = Vector2.ClampMagnitude(touchLocalPosition, joystickBase.rect.width / 2);
                // joystickPosition = touch.position - (Vector2)joystickBase.position;
                // joystickKnob.localPosition = Vector2.ClampMagnitude(joystickPosition, joystickBase.rect.width / 2);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isTouching = false;
                // joystickBase.gameObject.SetActive(false);
                joystickBase.localPosition = positionBase;
                joystickKnob.localPosition = positionKnob;
                onScreenStick.localPosition = positionStick;
            }
        }
    }

    public Vector2 GetJoystickDirection()
    {
        return onScreenStick.localPosition.normalized;
    }
}
