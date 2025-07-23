using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ConnectionHandler connectionHandler;

    [Space(20)]
    [Header("Prefabs to be Spawned")]
    [SerializeField] private QuizManager quizManager;

    internal void RegisterToolManager(ToolManager toolManager)
    {
        this.toolManager = toolManager;
    }

    [SerializeField] private InstructionManager instructionsPanel;
    [SerializeField] private ResultsPanel resultsPanel;

    [Space(20)]
    [Header("All Available Scenarios in project")]
    [SerializeField] private List<SceneData> sceneCollection;

    [Space(20)]
    [SerializeField]
    private float zAxisOffsetForUI, xAxisOffsetForUI;


    [SerializeField] private RandomScenarioSelector selector = new RandomScenarioSelector();
    [SerializeField] private PrefabSpawner spawner = new PrefabSpawner();
    private ISceneLoader sceneLoader = new SceneLoader();


    private SaveInformation saveInformation;

    [SerializeField]
    private QuizManager spawnedQuizUI;

    [SerializeField]
    private ToolManager toolManager;
    private IReadOnlyList<SceneData> chosenScenarios;
    private int current = -1;
    private Timer timer = new Timer();
    private ITerminateSession terminateSession;
    private Results resultWrapper = new Results();

    [SerializeField]
    private FPSDisplay fPSDisplay;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        connectionHandler.OnConnected += InitializeInstructions;
    }

    public List<SceneData> GetAllSceneData()
    {
        return sceneCollection;
    }
    public void InitializeInstructions(IGetTotalScenes getTotalScenes,bool isConnectionEstablished, ITerminateSession terminateSession)
    {
        if (isConnectionEstablished)
        {
            gameObject.AddComponent<SessionObserver>();
        }
        saveInformation = new SaveInformation();
        saveInformation.SaveData("Start Time", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        var instructions = spawner.Spawn<InstructionManager>(instructionsPanel.gameObject);
        instructions.GetComponent<RecenterController>().RecenterSelf();
        if (isConnectionEstablished)
        {
            fPSDisplay = FindAnyObjectByType<FPSDisplay>();
            fPSDisplay.SetSaveInfo(saveInformation, "Instructions Scene");
        }
        instructions.OnComplete += () =>
        {
            chosenScenarios = selector.Select(sceneCollection, getTotalScenes.GetNumberOfScenesRequired());
            NextScenario();
        };
        this.terminateSession = terminateSession;
    }

    [ContextMenu("Next scene")]
    private void NextScenario()
    {
        current++;
        Debug.Log("Scenario loading: " + current);
        if (current < chosenScenarios.Count)
        {
            StartCoroutine(RunScenario(chosenScenarios[current]));
        }
        else
        {
            ShowCompletion();
        }
    }

    private Vector3 lastPositon;
    private Quaternion lastRotation;
    private bool firstTime = true;
    private IEnumerator RunScenario(SceneData data)
    {
        Debug.Log(data.SceneIndex + " - Loading Scene");
        if(spawnedQuizUI != null)
        {
            lastPositon = spawnedQuizUI.transform.position;
            lastRotation = spawnedQuizUI.transform.rotation;
        }
        yield return sceneLoader.LoadScene(data.SceneIndex);
        SaveSceneInfo(data.sceneInformation.ScenarioName, data.sceneInformation.ScenarioDescription);
        timer.Start();
        if(GetComponent<SessionObserver>() != null)
        {
            fPSDisplay = FindAnyObjectByType<FPSDisplay>();
            fPSDisplay.SetSaveInfo(saveInformation, GetSceneNameByBuildIndex(data.SceneIndex));
        }
        //Spawn the quiz prefab & and get references for components
        spawnedQuizUI = spawner.Spawn<QuizManager>(quizManager.gameObject);
        var uiManager = spawnedQuizUI.GetComponent<UIManager>();

        uiManager.UpdateUIElements(data.sceneInformation,current+1);

        if (firstTime)
        {
            firstTime = false;
            //Get OVRManager parent to move quiz prefab to slightly left
            OVRManager ovrManager = FindAnyObjectByType<OVRManager>();
            IUIPositioner uiPositioner = new OVRUIPositioner(Camera.main.transform, zAxisOffsetForUI, xAxisOffsetForUI);
            uiPositioner.Position(spawnedQuizUI.transform);
        }
        else
        {
            spawnedQuizUI.transform.position = lastPositon;
            spawnedQuizUI.transform.rotation = lastRotation;
        }
        
        //Send question data to QuizManager
        //List<IFetechQuestionData> questionsData = data.QuestionRecords.Cast<IFetechQuestionData>().ToList();
        List<IFetechQuestionData> questionsData = new List<IFetechQuestionData>();
        foreach(var qr in data.QuestionRecords)
        {
            QuestionEvaluator qe = new QuestionEvaluator(qr as IQuestionDataProvider);
            questionsData.Add(qe);
        }
        Debug.Log(questionsData.Count() + " Converted questions");
        
        spawnedQuizUI.InitializeScenario(questionsData, resultWrapper, saveInformation, current == chosenScenarios.Count-1,data.sceneInformation.ScenarioName,current+1);
        spawnedQuizUI.OnComplete += () =>
        {
            SendScenarioCompletionStatements();
            NextScenario();
        };
    }
    public static string GetSceneNameByBuildIndex(int buildIndex)
    {
        // 1) Get the path (e.g. "Assets/Scenes/Level2City.unity")
        string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning($"No scene at build index {buildIndex}.");
            return string.Empty;
        }

        // 2) Strip off folders and the “.unity” extension
        return Path.GetFileNameWithoutExtension(path);
    }
    private void SendScenarioCompletionStatements()
    {
        float totalTimeSpent = timer.Stop();
        int currentSceneNumber = current + 1;
        saveInformation.SaveData("TimeSpent_"+ currentSceneNumber, totalTimeSpent.ToString("F2"));
        saveInformation.SaveData("MeasurementsReviewed_"+ currentSceneNumber, toolManager.IsToolActivatedAtleastOnce(ToolType.Measurement).ToString());

        float radiusWalked = FindAnyObjectByType<TreeGazeTracker>().GetReviewedPercent();
        saveInformation.SaveData("RadiusWalked_"+ currentSceneNumber, radiusWalked.ToString("F2") +"%");

    }
    [ContextMenu("Show completion")]
    private void ShowCompletion()
    {
        var quizUIPosition = spawnedQuizUI.transform.position;
        var quizUIRotation = spawnedQuizUI.transform.rotation;
        Destroy(spawnedQuizUI.gameObject);

        ResultsPanel resultsUIInstance = Instantiate(resultsPanel);
        resultsUIInstance.SetTerminateSession(terminateSession);
        resultsUIInstance.transform.position = quizUIPosition;
        resultsUIInstance.transform.rotation = quizUIRotation;
        int totalScenes = resultWrapper.AllResults.Count;
        int correctScenarios = 0;

        if(resultWrapper == null)
        {
            Debug.Log("Null: ");
            return;
        }
        Debug.Log(resultWrapper.AllResults.Count + " :Results count");
        foreach (var result in resultWrapper.AllResults)
        {
            if (result.IsScenarioPassed)
            {
                correctScenarios++;
            }
        }

        resultsUIInstance.ShowFinalScore(correctScenarios, totalScenes);
        Debug.Log("All scenes completed!");
    }
    void SaveSceneInfo(string scenarioName,string sceneDescription)
    {
        saveInformation.SaveData("Scenario Name_"+ (current+1), scenarioName);
        saveInformation.SaveData("Scenario Description_"+(current+1), sceneDescription);
    }
}

public class Results : IResults
{
    public List<Result> AllResults { get; set; } = new List<Result>();

    public void AddResult(Result result)
    {
        AllResults.Add(result);
    }
}
