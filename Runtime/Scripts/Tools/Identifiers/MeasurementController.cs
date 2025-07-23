using System.Collections.Generic;
using UnityEngine;

public class MeasurementController : MonoBehaviour
{

    [SerializeField] private List<Animator> animators;

    [ContextMenu("Open")]
    public void Open()
    {
        foreach(var anim in animators)
            anim.SetTrigger("OpenMeasurements");
    }

    // Call this to play in reverse:
    [ContextMenu("Close")]
    public void Close()
    {
        foreach(var anim in animators)
            anim.SetTrigger("CloseMeasurements");
    }

}