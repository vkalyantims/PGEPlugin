using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scenarioName;
    [SerializeField] private TextMeshProUGUI scenarioDescription;
    [SerializeField] private TextMeshProUGUI treeSpecies;
    [SerializeField] private TextMeshProUGUI treeHeight;
    [SerializeField] private TextMeshProUGUI treeDBH;
    [SerializeField] private TextMeshProUGUI treeGrowthRate;
    [SerializeField] private TextMeshProUGUI timeFrameInMonths;
    [SerializeField] private TextMeshProUGUI clearance;
    [SerializeField] private TextMeshProUGUI cleranceRequirments;
     

    public void UpdateUIElements(SceneInformation sceneInformation, int scenarioNumber)
    {
        scenarioName.text = "Scenario Number: " + scenarioNumber.ToString();
        scenarioDescription.text = "Scenario Description: " + sceneInformation.ScenarioDescription;
        treeSpecies.text = "Tree Species: " + sceneInformation.TreeSpecies;
        treeHeight.text = "Tree Height: " + sceneInformation.TreeHeight;
        treeDBH.text = "Tree DBH: " + sceneInformation.TreeDBH;
        treeGrowthRate.text = "Tree Growth Rate: "+sceneInformation.TreeGrowthRate;
        timeFrameInMonths.text = "Time to next evaluation: " + sceneInformation.TimeFrameInMonths + " months";
        clearance.text = "Clearance: " + sceneInformation.Clearance;
        cleranceRequirments.text = "Clearance Requirments: " + sceneInformation.ClearanceRequirments;
    }
}
