using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/RecenterEventChannel")]
public class RecenterEventChannelSO : ScriptableObject
{
    public UnityEvent OnRecenterRequested;

    public void Raise()
    {
        OnRecenterRequested?.Invoke();
    }
}
