using UnityEngine.UI;

public interface ITool
{
    ToolType ToolType { get; }
    Toggle Toggle { get; }
    bool IsActive { get; }
    bool IsActivatedAtleastOnce { get; }
    void Activate();
    void Deactivate();
}