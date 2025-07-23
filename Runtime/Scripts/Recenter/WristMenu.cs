using Oculus.Interaction.Input;
using TMPro;
using UnityEngine;

public class WristMenu : MonoBehaviour
{
    private Hand hand;
    private Pose currentPose;

    // Offset values to position the wrist menu more comfortably
    public Vector3 positionOffset = new Vector3(0.03f, 0.04f, -0.3f); // Adjust this according to needs
    public Vector3 rotationOffset = new Vector3(90, 0, 90); // Adjust rotation for better alignment

    private HandJointId handJointId = HandJointId.HandWristRoot;

    public GameObject menuButton;


    public Camera mainCamera; // Reference to the camera

    // The maximum allowed angle between the wrist's normal and the camera
    public float maxAngle = 135f;

    // Start is called before the first frame update
    void Start()
    {
        hand = this.gameObject.GetComponentInParent<Hand>();
        this.gameObject.transform.SetParent(hand.transform, false);
        if (!mainCamera)
        {
            mainCamera = Camera.main; // Automatically assign the main camera if not set
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (hand.GetJointPose(handJointId, out currentPose))
        {
            // Convert world position to local position relative to the hand
            this.gameObject.transform.localPosition = this.transform.parent.InverseTransformPoint(currentPose.position + currentPose.rotation * positionOffset);

            // Convert world rotation to local rotation relative to the hand
            this.gameObject.transform.localRotation = Quaternion.Inverse(this.transform.parent.rotation) * currentPose.rotation * Quaternion.Euler(rotationOffset);

            // Check the angle between the wrist normal and the camera direction
            Vector3 wristNormal = transform.forward; // Forward direction of the wrist
            Vector3 directionToCamera = (mainCamera.transform.position - currentPose.position).normalized; // Vector from wrist to camera

            // Calculate the angle between the wrist normal and the camera direction
            float angle = Vector3.Angle(wristNormal, directionToCamera);

            // Show or hide the wrist menu based on the angle
            if (angle > maxAngle)
            {
                menuButton.SetActive(true); // Show if the angle is greater than the max angle
            }
            else
            {
                menuButton.SetActive(false); // Hide if the angle is within the max angle
            }
        }
    }
}