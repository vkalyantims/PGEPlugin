using PGE.Diagnosis;
using UnityEngine;

[System.Serializable]
public class Option : IOption
{
    [SerializeField]
    private OptionData optionData;

    [SerializeField]
    private QuestionData dependentQuestion;

    [SerializeField]
    private OptionData requiredAnswer;

    public IOptionData OptionData => optionData;
    public IQuestionData DependentQuestion => dependentQuestion;
    public IOptionData RequiredAnswer => requiredAnswer;
}

public interface IOption
{
    IOptionData OptionData { get; }
    IQuestionData DependentQuestion { get; }
    IOptionData RequiredAnswer { get; }
}
