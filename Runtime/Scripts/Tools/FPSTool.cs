using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSTool : BaseTool
{
    FPSDisplay fPSDisplay;
    protected override void Awake()
    {
        base.Awake();
        fPSDisplay = FindFirstObjectByType<FPSDisplay>();
    }
    protected override void OnActivated()
    {
        fPSDisplay?.Enable();
    }

    protected override void OnDeactivated()
    {
        fPSDisplay?.Disable();
    }
}
