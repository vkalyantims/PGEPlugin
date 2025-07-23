using PGE.Diagnosis;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TTSManager))]
public class QuizManager : MonoBehaviour, IViewController
{
    [Space(20)]
    [Header("UI Elements in 1st Page (Choice Selection Panel)")]
    [SerializeField] private GameObject choiceSelectionPanel;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private ToggleGroup optionsGroup;
    [SerializeField] private SelectableOption optionPrefab;
    [SerializeField] private Button confirmSelection;

    [Space(20)]
    [Header("UI Elements in 2nd Page (Confirmation Panel)")]
    [SerializeField] private GameObject choiceConfirmationPanel;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button yesButton, NoButton;

    [Space(20)]
    [Header("UI Elements in 3rd Page (Load next scene panel)")]
    [SerializeField] private GameObject scenarioCompletePanel;
    [SerializeField] private Button continueToNextScenario;

    [Space(20)]
    [SerializeField]
    private PGE.Diagnosis.QuestionData diagnosisQuestion;

    [SerializeField] private TTSManager ttsManager;

    private IOptionData currentSelectedOption;
    private bool isCurrentScenarioLastInSession = false;
    public event Action OnComplete;
    private QuizEngine quizEngine;
    private void Awake()
    {
        ttsManager = GetComponent<TTSManager>();
    }
    private void OnToggleChanged(bool isOn, IOptionData option)
    {
        confirmSelection.onClick.RemoveAllListeners();
        if (isOn)
        {
            confirmationText.text = "You have selected " + option.optionText + ". Is this correct? ";
            confirmSelection.onClick.AddListener(() =>
                {
                    ttsManager.StopAudio();
                    choiceConfirmationPanel.SetActive(true);
                    choiceSelectionPanel.SetActive(false);
                }
            );
            currentSelectedOption = option;
        }
    }

    public void InitializeScenario(List<IFetechQuestionData> questionsList, IResults results, ISaveInformation saveInformation, bool isLastScenario, string scenarioName, int sceneNumber)
    {
        quizEngine = new QuizEngine(questionsList, saveInformation, scenarioName,sceneNumber, results);
        NoButton.onClick.AddListener(() =>
        {
            choiceSelectionPanel.SetActive(true);
            choiceConfirmationPanel.SetActive(false);

        });

        yesButton.onClick.AddListener(() =>
        {
            SubmitAnswer();
        });
        LoadQuestion();
        isCurrentScenarioLastInSession = isLastScenario;
    }

    void LoadQuestion()
    {
        IFetechQuestionData currentQuestion = quizEngine.GetCurrentQuestion();

        var diagnosisResult = quizEngine.GetSelectedOption(diagnosisQuestion);
        //Inject question text in field
        if (diagnosisResult != null)
        {
            questionText.text = currentQuestion.GetQuestionData().QuestionText.Replace("{diagnosis}", diagnosisResult.optionText);
        }
        else
        {
            questionText.text = currentQuestion.GetQuestionData().QuestionText.Replace("{diagnosis}", "diagnosis");
            Debug.LogWarning($"Diagnosis question result not found for key: {diagnosisQuestion}");
        }
        ttsManager.Speak(questionText.text);
        //Instantiate Options in toggle group
        foreach (Transform child in optionsGroup.transform)
        {
            Destroy(child.gameObject);
        }
        confirmSelection.onClick.RemoveAllListeners();
        foreach (var option in currentQuestion.GetAllOptions())
        {
            IOption currentOption = option;
            bool canShowThisOption = quizEngine.CanShowOption(option);

            if (canShowThisOption)
            {
                SelectableOption optionSpawned = Instantiate(optionPrefab, optionsGroup.transform);
                optionSpawned.SetOptionText(option.OptionData.optionText);

                Toggle toggle = optionSpawned.GetComponent<Toggle>();
                toggle.group = optionsGroup;
                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener((bool isOn) =>
                {
                    OnToggleChanged(isOn, option.OptionData);
                });
                toggle.gameObject.SetActive(true);
                toggle.isOn = false;
            }
        }
    }

    private void EnableScenarioCompletePanel()
    {
        choiceSelectionPanel.SetActive(false);
        choiceConfirmationPanel.SetActive(false);
        scenarioCompletePanel.SetActive(true);
        ttsManager.Speak("Great! You have completed your diagnosis of this scenario.");

        continueToNextScenario.onClick.RemoveAllListeners();
        continueToNextScenario.onClick.AddListener(() =>
        {
            continueToNextScenario.interactable = false;
            OnComplete?.Invoke();
        });
    }

    bool CanShowOption(IOption option)
    {
        if (option.DependentQuestion == null || option.RequiredAnswer == null)
        {
            return true;
        }

        if (quizEngine.CanShowOption(option))
        {
            return true;
        }

        return false;
    }
    void SubmitAnswer()
    {
        bool canFinishQuiz = quizEngine.ShouldEndQuiz(currentSelectedOption);
        quizEngine.ConfirmUserChoice(currentSelectedOption);
        Debug.Log("Can finish quiz - "+ canFinishQuiz +"Islastquestion - "+ quizEngine.IsLastQuestion);
        //quizEngine.GoToNextQuestion();
        if (canFinishQuiz || quizEngine.IsLastQuestion)
        {
            if (!isCurrentScenarioLastInSession)
            {
                EnableScenarioCompletePanel();
            }
            else
            {
                OnComplete?.Invoke();
            }
            return;
        }
        choiceConfirmationPanel.SetActive(false);
        choiceSelectionPanel.SetActive(true);
        if (!(canFinishQuiz || quizEngine.IsLastQuestion))
            LoadQuestion();
    }
}


