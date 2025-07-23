using PGE.Diagnosis;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class QuestionRecord :  IQuestionDataProvider
{
    [SerializeField]
    private QuestionData questionData;

    [SerializeField]
    private List<Option> customOptions;

    [SerializeField]
    private List<OptionData> acceptableOptions = new List<OptionData>();

    [SerializeField]
    private List<OptionData> idealOptions = new List<OptionData>();

    [SerializeField]
    private List<OptionData> finishEvaluationOptions = new List<OptionData>();


    public IQuestionData QuestionData => questionData;
    public List<IOption> CustomOptions => customOptions.Cast<IOption>().ToList();
    public List<IOptionData> AcceptableOptions => acceptableOptions.Cast<IOptionData>().ToList();
    public List<IOptionData> IdealOptions => idealOptions.Cast<IOptionData>().ToList();
    public List<IOptionData> FinishEvaluationOptions => finishEvaluationOptions.Cast<IOptionData>().ToList();
}


public interface IQuestionDataProvider
{
    IQuestionData QuestionData { get; }
    List<IOption> CustomOptions { get; }
    List<IOptionData> AcceptableOptions { get; }
    List<IOptionData> IdealOptions { get; }
    List<IOptionData> FinishEvaluationOptions { get; }
}