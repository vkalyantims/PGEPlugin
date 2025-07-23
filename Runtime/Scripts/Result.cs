public class Result
{
    public string ScenarioName;
    public bool IsScenarioPassed;
    public Result(string scenarioName, bool isScenarioPassed)
    {
        ScenarioName = scenarioName;
        IsScenarioPassed = isScenarioPassed;
    }
}