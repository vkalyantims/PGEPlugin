using UnityEngine;
using UnityEngine.UI;

public class ToolsController : MonoBehaviour
{
    MeasurementController measurementController;

    [SerializeField]
    private Toggle measureToggle;

    private void Start()
    {
        measurementController = FindFirstObjectByType<MeasurementController>();

        measureToggle.onValueChanged.AddListener((bool isOn) =>
        {
            if (isOn)
            {
                measurementController.Open();
            }
            else
            {
                measurementController.Close();
            }
        });
    }
}
