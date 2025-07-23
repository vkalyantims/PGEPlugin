using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TTSManager))]
public class InstructionManager : MonoBehaviour, IViewController
{

    [Header("UI References")]

    [SerializeField]
    private TextMeshProUGUI instructionText;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private TextMeshProUGUI buttonText;
    private int currentIndex = 0;

    [SerializeField]
    private Instructions instructions;

    [SerializeField] private TTSManager ttsManager;

    Action LoadNextScene;

    public event Action OnComplete;

    void Start()
    {
        ttsManager = GetComponent<TTSManager>();
        if (instructions == null || instructions.instructionTexts.Count == 0)
        {
            instructionText.text = "No instructions available.";
            nextButton.interactable = false;
            return;
        }

        nextButton.onClick.AddListener(OnNextClicked);
        ShowCurrentInstruction();
    }
    private void ShowCurrentInstruction()
    {
        instructionText.text = instructions.instructionTexts[currentIndex].text;
        ttsManager.Speak(instructionText.text);
    }

    private void OnNextClicked()
    {
        currentIndex++;

        if (currentIndex >= instructions.instructionTexts.Count)
        {
            nextButton.interactable = false;
            InstructionCompleted();
            return;
        }

        ShowCurrentInstruction();
    }

    internal void SetInitializeScenes(Action initializeScenes)
    {
        LoadNextScene = initializeScenes;
    }
    [ContextMenu("Complete Instructions")]
    private void InstructionCompleted()
    {
        ttsManager.StopAudio();
        LoadNextScene?.Invoke();
        OnComplete?.Invoke();
        Debug.Log("Instruction sequence finished.");
    }

}
