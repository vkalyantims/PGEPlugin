using System;
using UnityEngine;
using UnityEngine.UI;

public class WristButtonHandler : MonoBehaviour
{
    [SerializeField] private RecenterEventChannelSO recenterChannel;
    [SerializeField] private Toggle toggle;

    private void OnEnable()
    {
        toggle.onValueChanged.AddListener(OnWristButtonClicked);
    }

    private void OnWristButtonClicked(bool isOn)
    {
        recenterChannel?.Raise();
    }

    private void OnDisable()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }
}
