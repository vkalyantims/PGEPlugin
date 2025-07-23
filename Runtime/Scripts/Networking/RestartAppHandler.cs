using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FadeScreen))]
public class RestartAppHandler : MonoBehaviour
{
    [SerializeField]
    private Button quitButton;

    [SerializeField]
    private TextMeshProUGUI message;
    private FadeScreen fadeScreen;
    public void AddTerminateSessionEvent(Action terminate)
    {
        quitButton.onClick.AddListener(() =>
        {
            terminate?.Invoke();
        });
    }
    public void SetMessage(string msg)
    {
        message.text = msg;
    }
    public void FadeOut()
    {
        
        fadeScreen = GetComponent<FadeScreen>();
        if (fadeScreen != null)
            fadeScreen.FadeOut();

    }
}
