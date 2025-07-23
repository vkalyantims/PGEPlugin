using Oculus.Interaction;
using UnityEngine;

namespace Assets.Scripts.Animation
{
    [RequireComponent(typeof(RayInteractor))]
    [RequireComponent(typeof(LineRenderer))]
    public class RayInteractorVisual : MonoBehaviour
    {
        [SerializeField]
        private Color _rayColor = new Color(1f, 1f, 1f, 0.67f);

        [SerializeField]
        private float _lineWidth = 0.001f;

        [SerializeField]
        private HandPointerPose _handPointerPose;

        private RayInteractor _rayInteractor;

        [SerializeField]
        private Transform _rayOrigin;
        private LineRenderer _lineRenderer;

        [SerializeField]
        private float _offset = 0.055f;

        private void Awake()
        {
            _rayInteractor = GetComponent<RayInteractor>();
            _lineRenderer = GetComponent<LineRenderer>();

            _lineRenderer.positionCount = 2;
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.widthMultiplier = _lineWidth;

            var mat = new Material(Shader.Find("Unlit/Color"));
            mat.color = _rayColor;
            _lineRenderer.material = mat;
            _lineRenderer.enabled = false;

            if (_handPointerPose == null)
            {
                Debug.LogWarning("HandPointerPose reference not set. Ray line will never show.");
            }
        }

        private void Update()
        {
            _lineRenderer.widthMultiplier = _lineWidth;
            if (_handPointerPose != null && _handPointerPose.Active)
            {
                _lineRenderer.enabled = true;
                _lineRenderer.SetPosition(0, _rayOrigin.position + _rayOrigin.forward * _offset);
                _lineRenderer.SetPosition(1, _rayInteractor.End);
            }
            else
            {
                _lineRenderer.enabled = false;
            }
        }
    }
}
