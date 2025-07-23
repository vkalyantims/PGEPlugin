using UnityEngine;
using UnityEngine.Events;
public class OvrCustomButtonEvents : MonoBehaviour
{
    public enum ControllerButton
    {
        Left_Start,
        Left_One,
        Left_Two,
        Left_Thumbstick,
        Left_IndexTrigger,
        Left_SecondaryThumbstick,

        Right_Start,
        Right_One,
        Right_Two,
        Right_Thumbstick,
        Right_IndexTrigger,
        Right_SecondaryThumbstick
    }

    [Header("Button Settings")]
    public ControllerButton buttonToCheck = ControllerButton.Left_Start;

    [Header("Events")]
    public UnityEvent OnButtonPressed;
    public UnityEvent OnButtonReleased;

    private bool wasPressedLastFrame = false;

    void Update()
    {
        OVRInput.Button ovrButton = MapToOVRButton(buttonToCheck);
        OVRInput.Controller ovrController = MapToController(buttonToCheck);

        bool isPressed = OVRInput.Get(ovrButton, ovrController);

        if (isPressed && !wasPressedLastFrame)
        {
            OnButtonPressed.Invoke();
        }
        else if (!isPressed && wasPressedLastFrame)
        {
            OnButtonReleased.Invoke();
        }

        wasPressedLastFrame = isPressed;
    }

    OVRInput.Button MapToOVRButton(ControllerButton btn)
    {
        switch (btn)
        {
            case ControllerButton.Left_Start:
            case ControllerButton.Right_Start:
                return OVRInput.Button.Start;

            case ControllerButton.Left_One:
            case ControllerButton.Right_One:
                return OVRInput.Button.One;

            case ControllerButton.Left_Two:
            case ControllerButton.Right_Two:
                return OVRInput.Button.Two;

            case ControllerButton.Left_Thumbstick:
            case ControllerButton.Right_Thumbstick:
                return OVRInput.Button.PrimaryThumbstick;

            case ControllerButton.Left_SecondaryThumbstick:
            case ControllerButton.Right_SecondaryThumbstick:
                return OVRInput.Button.SecondaryThumbstick;

            case ControllerButton.Left_IndexTrigger:
            case ControllerButton.Right_IndexTrigger:
                return OVRInput.Button.PrimaryIndexTrigger;

            default:
                return OVRInput.Button.None;
        }
    }

    OVRInput.Controller MapToController(ControllerButton btn)
    {
        if (btn.ToString().StartsWith("Left"))
            return OVRInput.Controller.LTouch;
        else
            return OVRInput.Controller.RTouch;
    }
}
