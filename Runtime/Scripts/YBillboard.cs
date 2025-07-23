using UnityEngine;

[ExecuteAlways]
public class YBillboard : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        // grab the main camera (play mode or editor preview)
        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // recover the camera if it wasn’t assigned yet (e.g. in Edit mode)
        if (cameraTransform == null)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
            else
                return;
        }

        // 1) Compute the direction to the camera…
        Vector3 dir = cameraTransform.position - transform.position;
        // 2) …but ignore any vertical difference
        dir.y = 0f;

        // 3) If there’s any horizontal distance, face that way
        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }
}
