using System.Collections.Generic;

public interface IScenarioSelector
{
    IReadOnlyList<SceneData> Select(IReadOnlyList<SceneData> candidates, int count);
}