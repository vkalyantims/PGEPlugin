using PGE.Diagnosis;
using System.Collections.Generic;
using System.Linq;

public class QuestionEvaluator: IFetechQuestionData
{
    readonly IQuestionDataProvider _data;
    Dictionary<IOptionData, (IQuestionData, IOptionData)> _dependencyMap;

    public QuestionEvaluator(IQuestionDataProvider data)
    {
        _data = data;
        BuildDependencyMap();
    }
    public List<IOption> GetAllOptions()
    {
        if (_data.QuestionData.UseDefaultOptions)
        {
            return _data.QuestionData.AllOptions;
        }
        return _data.CustomOptions;
    }
    void BuildDependencyMap()
    {
        _dependencyMap = new();
        foreach (var opt in GetAllOptions())
        {
            if (opt.DependentQuestion != null && opt.RequiredAnswer != null)
                _dependencyMap[opt.OptionData] = (opt.DependentQuestion, opt.RequiredAnswer);
        }
    }

    public string GetAcceptableChoices(Dictionary<IQuestionData, IOptionData> results)
    {
        var filtered = new List<IOptionData>();
        foreach (var choice in _data.AcceptableOptions)
        {
            if (_dependencyMap.TryGetValue(choice, out var dep))
            {
                if (results.TryGetValue(dep.Item1, out var answer) && answer == dep.Item2)
                    filtered.Add(choice);
            }
            else
            {
                filtered.Add(choice);
            }
        }
        return filtered.Any()
            ? string.Join(", ", filtered.Select(x => x.optionText))
            : "Not Applicable";
    }

    public string GetIdealChoices(Dictionary<IQuestionData, IOptionData> results)
    {
        var filtered = new List<IOptionData>();
        foreach (var choice in _data.IdealOptions)
        {
            if (_dependencyMap.TryGetValue(choice, out var dep))
            {
                if (results.TryGetValue(dep.Item1, out var answer) && answer == dep.Item2)
                    filtered.Add(choice);
            }
            else
            {
                filtered.Add(choice);
            }
        }
        return filtered.Any()
            ? string.Join(", ", filtered.Select(x => x.optionText))
            : "Not Applicable";
    }

    public bool IsSelectedOptionIdealOrAcceptabe(IOptionData selected)
    {
        return _data.IdealOptions.Contains(selected)
            || _data.AcceptableOptions.Contains(selected);
    }

    public IQuestionData GetQuestionData()
    {
        return _data.QuestionData;
    }

    public bool IsThisQuestionPartOfEvaluation()
    {
        return _data.QuestionData.IsThisQuestionPartOfEvaluation;
    }

    public bool IsOptionPresentInFinishEvaluationList(IOptionData selectedOption)
    {
        return _data.FinishEvaluationOptions.Contains(selectedOption);
    }
}
