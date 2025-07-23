using System.Collections.Generic;
using UnityEngine;

public class ToolManager : MonoBehaviour
{
    private Dictionary<ToolType, ITool> tools = new();

    void Awake()
    {
        foreach (var tool in GetComponentsInChildren<ITool>(true))
        {
            tools[tool.ToolType] = tool;
            Debug.Log(tool.ToolType);
        }

        var gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.RegisterToolManager(this);
        }
    }

    public bool IsToolActivatedAtleastOnce(ToolType toolType)
    {
        return tools.TryGetValue(toolType, out var tool) && tool.IsActivatedAtleastOnce;
    }
}
