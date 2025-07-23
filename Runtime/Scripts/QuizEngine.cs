using PGE.Diagnosis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuizEngine
{
    private readonly List<IFetechQuestionData> questionsList;
    private int currentQuestionIndex = 0;
    public bool IsLastQuestion => currentQuestionIndex >= questionsList.Count;
    private List<bool> isCurrentScenarioPass = new List<bool>(); // Empty list at start
    public bool IsCurrentScenarioPass => isCurrentScenarioPass.Count > 0 && isCurrentScenarioPass.All(pass => pass);

    //All fields are below will be set via Constructor

    //Stores all the choices made by user
    private Dictionary<IQuestionData, IOptionData> userChoices = new Dictionary<IQuestionData, IOptionData>();
    private ISaveInformation saveInformation;
    private int sceneNumber;
    private string sceneName;
    private IResults resultsWrapper;

    public QuizEngine(List<IFetechQuestionData> questionsList, ISaveInformation saveInformation, string scenarioName,int sceneNumber, IResults results)
    {
        this.questionsList = questionsList;
        this.saveInformation = saveInformation;
        this.sceneNumber = sceneNumber;
        sceneName = scenarioName;
        resultsWrapper = results;
    }
    public IFetechQuestionData GetCurrentQuestion()
    {
        Debug.Log("Current index - " + currentQuestionIndex);
        return questionsList[currentQuestionIndex];
    }
    private void GoToNextQuestion()
    {
        currentQuestionIndex++;
        if (IsLastQuestion)
            EndQuiz();
    }

    public void SendNextQuestionsData()
    {
        for (int questionIndex = currentQuestionIndex + 1; questionIndex < questionsList.Count; questionIndex++)
        {
            SaveQuestionResult(questionsList[questionIndex], null);
        }
    }

    public void SaveQuestionResult(IFetechQuestionData question, IOptionData currentOption)
    {
        QuestionResult result = new QuestionResult();
        result.questionType = question.GetQuestionData().QuestionPrefix;
        result.acceptableChoices = question.GetAcceptableChoices(userChoices);
        result.idealChoices = question.GetIdealChoices(userChoices);
        result.userChoice = currentOption != null ? currentOption.optionText : "Not Applicable";
        result.result = currentOption != null ? question.IsSelectedOptionIdealOrAcceptabe(currentOption) : IsCurrentScenarioPass;
        result.sendBothIdealAcceptable = question.GetQuestionData().ShouldSendIdealAndAcceptableStatements;
        saveInformation.SaveQuestionData(result, sceneNumber);
    }

    public bool ConfirmUserChoice(IOptionData chosenOption)
    {
        var currentQuestion = GetCurrentQuestion();
        userChoices[currentQuestion.GetQuestionData()] = chosenOption;
        SaveQuestionResult(currentQuestion, chosenOption);

        if (currentQuestion.IsThisQuestionPartOfEvaluation())
        {
            isCurrentScenarioPass.Add(IsUserChoiceCorrect(currentQuestion, chosenOption));
        }
        if (ShouldEndQuiz(chosenOption))
        {
            SendNextQuestionsData();
            EndQuiz();
        }
        if (!currentQuestion.IsOptionPresentInFinishEvaluationList(chosenOption) && !IsLastQuestion)
        {
            GoToNextQuestion();
        }
        return IsUserChoiceCorrect(currentQuestion,chosenOption);
        
    }
    public bool ShouldEndQuiz(IOptionData chosenOption)
    {
        return GetCurrentQuestion().IsOptionPresentInFinishEvaluationList(chosenOption);
    }
    private bool IsUserChoiceCorrect(IFetechQuestionData currentQuestion, IOptionData chosenOption)
    {
        return currentQuestion.IsSelectedOptionIdealOrAcceptabe(chosenOption);
    }
    private void EndQuiz()
    {
        Result result = new Result(sceneName, IsCurrentScenarioPass);
        resultsWrapper.AddResult(result);
    }

    public IOptionData GetSelectedOption(IQuestionData question)
    {
        return userChoices.TryGetValue(question, out IOptionData result)? result:null;
    }

    public bool CanShowOption(IOption option)
    {
        if (option.DependentQuestion == null || option.RequiredAnswer == null)
        {
            return true;
        }
        return userChoices[option.DependentQuestion] == option.RequiredAnswer;
    }
}
