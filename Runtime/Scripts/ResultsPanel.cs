using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TTSManager))]
public class ResultsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    //[SerializeField] private TextMeshProUGUI Result_Text;
    [SerializeField] private Button quitApplication;
    [SerializeField] private Button nextPage;
    [SerializeField] private TTSManager ttsManager;
    private ITerminateSession terminateSession;

    public void SetTerminateSession(ITerminateSession terminateSession)
    {
        this.terminateSession = terminateSession;
    }
    private void Start()
    {

        quitApplication.gameObject.SetActive(false);
        nextPage.gameObject.SetActive(true);

        nextPage.onClick.AddListener(() =>
        {
            scoreText.text = "Your assessor can provide more information about your results and any applicable next steps.";
            ttsManager.Speak(scoreText.text);

            quitApplication.gameObject.SetActive(true);
            nextPage.gameObject.SetActive(false);
        });


        quitApplication.onClick.AddListener(() => terminateSession.TerminateSession());
    }
    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }

    public void ShowFinalScore(int correctAnswers, int totalQuestions)
    {
        ttsManager = GetComponent<TTSManager>();
        string scenarioText = totalQuestions == 1 ? "scenario" : "scenarios";
        string resultText = $"This concludes the assessment.Based on your answers, you scored {correctAnswers} out of {totalQuestions} {scenarioText} in this session.";
        scoreText.text = resultText;
        ttsManager.Speak(resultText);
        string result_text = string.Empty;
        //foreach (Result result in results)
        //{
        //    string resultStatus;
        //    if (result.IsScenarioPassed)
        //        resultStatus = "PASS";
        //    else resultStatus = "FAIL";

        //    result_text += result.ScenarioName + " : " + resultStatus + "\n";
        //}
        //Result_Text.text = result_text.Remove(result_text.Length - 1); // Remove the last newline character

    }
}
