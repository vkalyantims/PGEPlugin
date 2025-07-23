using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    
    void Update()
    {
        Camera.main.fieldOfView = 30;
    }
    private void LateUpdate()
    {
        Camera.main.fieldOfView = 30;
    }
    private void FixedUpdate()
    {
        Camera.main.fieldOfView = 30;
    }
}
