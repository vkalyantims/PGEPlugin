using System.Collections.Generic;

public interface IFetechQuestionData
{
    public IQuestionData GetQuestionData();
    public List<IOption> GetAllOptions();
    public bool IsThisQuestionPartOfEvaluation();
    public bool IsOptionPresentInFinishEvaluationList(IOptionData selectedOption);

    public bool IsSelectedOptionIdealOrAcceptabe(IOptionData selectedOption);

    public string GetAcceptableChoices(Dictionary<IQuestionData, IOptionData> results);
    public string GetIdealChoices(Dictionary<IQuestionData, IOptionData> results);
}