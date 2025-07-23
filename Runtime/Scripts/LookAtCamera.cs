using UnityEngine;

[ExecuteAlways]
public class LookAtCamera : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (cameraTransform == null)
        {
            return;
        }

        transform.LookAt(cameraTransform);
        transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
    }
}
