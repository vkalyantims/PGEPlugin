using UnityEngine;

public class MeasurementTool : BaseTool
{
    MeasurementController measurementController;
    protected override void Awake()
    {
        base.Awake();
        measurementController = FindFirstObjectByType<MeasurementController>();
    }
    [ContextMenu("Activate")]
    protected override void OnActivated()
    {
        measurementController.Open();
    }
    [ContextMenu("DeActivate")]
    protected override void OnDeactivated()
    {
        measurementController.Close();
    }
}
