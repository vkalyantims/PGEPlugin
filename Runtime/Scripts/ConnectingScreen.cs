using TMPro;
using UnityEngine;

[RequireComponent(typeof(RecenterController))]
public class ConnectingScreen : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI connectingScreenStatusText;

    private void Start()
    {
        var recenterController = GetComponent<RecenterController>();
        if (recenterController != null)
        {
            recenterController.RecenterSelf();
        }
        else
        {
            Debug.LogWarning("RecenterController component not found on ConnectingScreen.");
        }
    }

    public void SetConnectionStatus(string text)
    {
        connectingScreenStatusText.text = text;
    }

}
