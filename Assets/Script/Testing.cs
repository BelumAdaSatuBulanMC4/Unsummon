using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoystickGame1 : MonoBehaviour
{
    public RectTransform joystickBase;
    public RectTransform joystickKnob;
    public RectTransform onScreenStick; // Reference to the child object
    public TMP_Text tulisan;
    private Vector2 joystickPosition = Vector2.zero;
    private bool isTouching = false;

    private Vector2 initialKnobPosition;
    private Vector2 initialBasePosition;

    private void Start()
    {
        initialBasePosition = joystickBase.localPosition;
        initialKnobPosition = joystickKnob.localPosition;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x < Screen.width / 2)
            {
                if (!isTouching)
                {
                    // Initial touch on the left side of the screen
                    isTouching = true;
                    joystickBase.gameObject.SetActive(true);
                    joystickKnob.gameObject.SetActive(true);
                    joystickBase.position = touch.position;
                    joystickKnob.position = touch.position;
                }

                // Update joystick knob position relative to joystick base
                Vector2 touchLocalPosition = (Vector2)joystickBase.InverseTransformPoint(touch.position);
                joystickKnob.localPosition = Vector2.ClampMagnitude(touchLocalPosition, joystickBase.rect.width / 2);

                // Update the onScreenStick position to follow the knob
                if (onScreenStick != null)
                {
                    onScreenStick.localPosition = joystickKnob.localPosition;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // End touch, reset joystick to initial positions
                isTouching = false;
                joystickBase.localPosition = initialBasePosition;
                joystickKnob.localPosition = initialKnobPosition;

                // Reset the onScreenStick position as well
                if (onScreenStick != null)
                {
                    onScreenStick.localPosition = initialKnobPosition;
                }

                tulisan.text = "x: " + GetJoystickDirection().x + " dan y:" + GetJoystickDirection().y;

                joystickBase.gameObject.SetActive(false);
                joystickKnob.gameObject.SetActive(false);
            }
        }
    }

    public Vector2 GetJoystickDirection()
    {
        // Normalize joystick direction for movement or other inputs
        return onScreenStick.localPosition.normalized;
    }
}
