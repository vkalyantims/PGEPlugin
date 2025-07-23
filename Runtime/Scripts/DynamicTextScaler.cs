using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DynamicTextScaler : MonoBehaviour
{
    private Camera targetCamera;

    private Vector3 _initialScale;
    private float _initialDistance;

    void Awake()
    {
        _initialScale = transform.localScale;

        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    void Start()
    {
        if (targetCamera != null)
        {
            _initialDistance = Vector3.Distance(
                targetCamera.transform.position,
                transform.position
            );
            if (_initialDistance < 0.01f)
                _initialDistance = 1f; // avoid divide-by-zero
        }
        else
        {
            _initialDistance = 1f;
        }
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        float currentDistance = Vector3.Distance(
            targetCamera.transform.position,
            transform.position
        );

        // scale factor before clamping
        float factor = currentDistance / _initialDistance;

        factor = Mathf.Max(1,factor);

        transform.localScale = _initialScale * factor;
    }
}
