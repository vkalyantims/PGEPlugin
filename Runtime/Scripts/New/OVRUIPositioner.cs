using UnityEngine;

public class OVRUIPositioner : IUIPositioner
{
    private readonly Transform _anchor;
    private readonly float _zOffset;
    private readonly float _xOffset;

    public OVRUIPositioner(Transform ovrParent, float zAxisOffset, float xAxisOffset)
    {
        _anchor = ovrParent;
        _zOffset = zAxisOffset;
        _xOffset = xAxisOffset;
    }

    public void Position(Transform uiTransform)
    {
        if (_anchor == null || uiTransform == null) return;

        // replicate your snippet:
        Vector3 frontPos = _anchor.forward * _zOffset;
        Vector3 leftPos = _anchor.right * _xOffset;
        Vector3 spawnPos = _anchor.position + frontPos + leftPos;
        Quaternion rot = Quaternion.LookRotation(spawnPos - _anchor.position);

        uiTransform.position = new Vector3(spawnPos.x, uiTransform.position.y, spawnPos.z);
        uiTransform.rotation = rot;
    }
}
