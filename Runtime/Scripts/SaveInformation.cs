using EndeaVR.Scripts;
using System;
using UnityEngine;

public class SaveInformation : ISaveInformation
{
    public void SaveData(string variableName, string result)
    {
        var _evrClient = EVRClient.Instance;
        if (_evrClient != null)
        {
            _evrClient.PostResults(
                new EndeaVR.SDK.Unity.Models.Result[] { 
                    new EndeaVR.SDK.Unity.Models.Result() { 
                        CreatedDate = DateTime.Now, OldValue = "", 
                        NewValue = result, VariableName = variableName
                    } 
                },
                onSuccess: (msg) => {
                    Debug.Log(msg);
                },
                onError: (error) => {
                    Debug.Log(variableName + " " + result + "- Not saved");
                    Debug.Log(error);
                }
            );
        }
        else
        {
            Debug.Log("EVR Client is null");
        }
    }

    public void SaveQuestionData(QuestionResult questionResult, int sceneNumber)
    {
        if (questionResult.sendBothIdealAcceptable)
        {
            SaveData(questionResult.questionType+ "_Expected_Acceptable" +"_"+sceneNumber, questionResult.acceptableChoices);
            SaveData(questionResult.questionType+ "_Expected_Ideal" + "_" + sceneNumber, questionResult.idealChoices);
        }
        else
        {
            SaveData(questionResult.questionType + "_Expected" + "_" + sceneNumber, questionResult.idealChoices);
        }
        SaveData(questionResult.questionType + "_Response" + "_" + sceneNumber, questionResult.userChoice);
        SaveData(questionResult.questionType + "_Result" + "_" + sceneNumber, questionResult.result.ToString());
    }
}

public interface ISaveInformation
{
    public void SaveData(string variableName, string result);
    public void SaveQuestionData(QuestionResult questionResult, int sceneNumber);
}
