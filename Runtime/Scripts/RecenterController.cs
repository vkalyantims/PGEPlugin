using UnityEngine;


public class RecenterController : MonoBehaviour
{
    [Tooltip("The distance (in units) at which the object will be placed in front of the camera.")]
    public float recenterDistance = 2.0f;

    [Tooltip("The OVR button to trigger recentering (e.g., OVRInput.Button.Start).")]
    public OVRInput.Button recenterButton = OVRInput.Button.Start;

    [Tooltip("The controller to check for the recentering input (e.g., OVRInput.Controller.LHand).")]
    public OVRInput.Controller recenterController = OVRInput.Controller.LHand;

    [SerializeField]
    private RecenterEventChannelSO recenterChannel;

    [SerializeField]
    private Collider canvasCollider, treeCollider;
    private void OnEnable()
    {
        canvasCollider = GetComponentInChildren<Collider>();
        treeCollider = FindObjectOfType<Tree>().GetComponent<Collider>();
        if (recenterChannel != null)
            recenterChannel.OnRecenterRequested.AddListener(RecenterSelf);
    }

    private void OnDisable()
    {
        if (recenterChannel != null)
            recenterChannel.OnRecenterRequested.RemoveListener(RecenterSelf);
    }

    // Update is called once per frame.
    private void Update()
    {
        // Check if the recenter button is pressed.
        if (OVRInput.GetDown(recenterButton, recenterController))
        {
            // Recenter this game object.
            Recenter(transform);
        }
    }

    /// <summary>
    /// Repositions the given transform to be directly in front of the main camera
    /// on the object's current horizontal plane (preserving its y coordinate) at recenterDistance,
    /// and rotates the object so that its forward vector on the y-axis is rotated by 180 degrees from the direction toward the camera.
    /// </summary>
    /// <param name="objectToRecenter">The transform to reposition and rotate.</param>
    /// 
    [ContextMenu("Recenter ui")]
    public void RecenterSelf()
    {
        Recenter(this.transform);
    }
    public void Recenter(Transform objectToRecenter)
    {
        Camera cam = Camera.main;
        if (cam == null)
            return;

        // Preserve the object's current y coordinate.
        float currentY = objectToRecenter.position.y;

        // Get the camera's position projected onto the plane at currentY.
        Vector3 camPos = cam.transform.position;
        Vector3 camPosOnPlane = new Vector3(camPos.x, currentY, camPos.z);

        // Determine the camera's forward direction on the x-z plane.
        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        // Compute the new position on the plane at the specified recenterDistance.
        Vector3 newPosOnPlane = camPosOnPlane + camForward * recenterDistance;

        // Set the object's new position, preserving its y coordinate.
        objectToRecenter.position = new Vector3(newPosOnPlane.x, currentY, newPosOnPlane.z);

        // Compute the direction from the object to the camera on the x-z plane.
        Vector3 directionToCamera = camPosOnPlane - objectToRecenter.position;

        // Calculate the angle in degrees (from the object's position to the camera) on the x-z plane.
        float desiredYAngle = Mathf.Atan2(directionToCamera.x, directionToCamera.z) * Mathf.Rad2Deg;

        // Rotate the object on the y axis by adding 180 degrees.
        desiredYAngle += 180f;

        // Preserve the object's current x and z rotation components.
        Quaternion currentRot = objectToRecenter.rotation;
        Quaternion newRot = Quaternion.Euler(currentRot.eulerAngles.x, desiredYAngle, currentRot.eulerAngles.z);
        objectToRecenter.rotation = newRot;
        if(canvasCollider && treeCollider)
            UpdatePositionToAvoidOverlap(canvasCollider, treeCollider, objectToRecenter);
    }

    private void UpdatePositionToAvoidOverlap(Collider colliderA, Collider rigidCollider, Transform objectToRecenter)
    {

        Vector3 direction;
        float distance;

        if (Physics.ComputePenetration(
            colliderA, colliderA.transform.position, colliderA.transform.rotation,
            rigidCollider, rigidCollider.transform.position, rigidCollider.transform.rotation,
            out direction, out distance))
        {
            // Move colliderA out of colliderB
            objectToRecenter.position += direction * distance;
        }
    }
}
