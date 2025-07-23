using UnityEngine;
using UnityEngine.UI;

public abstract class BaseTool : MonoBehaviour, ITool
{
    [SerializeField] private ToolType toolType;
    [SerializeField] private Toggle toggle;
    public ToolType ToolType => toolType;

    public bool IsActive { get; private set; }
    public bool IsActivatedAtleastOnce { get; private set; }

    public Toggle Toggle => toggle;
    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
        IsActivatedAtleastOnce = true;
        OnActivated();
    }

    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
        OnDeactivated();
    }

    protected abstract void OnActivated();
    protected abstract void OnDeactivated();

    protected virtual void Awake()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    private void OnToggleChanged(bool isOn)
    {
        if (isOn) Activate();
        else Deactivate();
    }

    protected virtual void OnDestroy()
    {
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }
}
