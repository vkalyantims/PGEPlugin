using UnityEngine;

public interface IUIPositioner
{
    /// <summary>
    /// Repositions the given UI transform in front of the given OVRCamera parent.
    /// </summary>
    void Position(Transform uiTransform);
}
