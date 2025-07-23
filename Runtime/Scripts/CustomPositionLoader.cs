using UnityEngine;
using UnityEngine.SceneManagement;
public class CustomPositionLoader : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    OVRCameraRig cameraRig;
    // Start is called before the first frame update
    private void Awake()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;

        cameraRig = gameObject.GetComponent<OVRCameraRig>();

        if (targetTransform != null)
            ChangePosition();
    }


    void ChangePosition()
    {
        if (transform.parent != null && cameraRig != null)
        {
            Transform centerCamera = cameraRig.centerEyeAnchor.transform;

            transform.parent.position = targetTransform.position;
            //centerCamera.localPosition = Vector3.zero;
            centerCamera.rotation = targetTransform.rotation;
        }

    }

}
