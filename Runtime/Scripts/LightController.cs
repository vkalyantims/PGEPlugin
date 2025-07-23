using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class LightController : MonoBehaviour
{
    LightSource lightSource;
    private void Awake()
    {
        lightSource = FindAnyObjectByType<LightSource>();

        if (lightSource)
        {
            var toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((bool isOn) =>
            {

                lightSource.gameObject.GetComponent<Light>().enabled = isOn;
                
            });
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
