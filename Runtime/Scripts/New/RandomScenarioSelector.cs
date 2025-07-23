using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomScenarioSelector : IScenarioSelector
{
    private const int DEFAULT_SCENES_COUNT = 5;
    public IReadOnlyList<SceneData> Select(IReadOnlyList<SceneData> fullSceneData, int count)
    {
        int sceneCount = count;
        if (sceneCount <= 0)
        {
            sceneCount = DEFAULT_SCENES_COUNT;
        }
        sceneCount = Mathf.Min(sceneCount, fullSceneData.Count);

        return fullSceneData
        .Distinct()
        .OrderBy(_ => Random.value)
        .Take(sceneCount)
        .ToList();
    }
}

