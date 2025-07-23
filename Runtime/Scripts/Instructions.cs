using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Instructions", menuName = "Instructions for PGE/Instructions")]
public class Instructions : ScriptableObject
{
    public List<InstructionText> instructionTexts = new List<InstructionText>();
}

[System.Serializable]
public class InstructionText
{
    [TextArea]
    public string text;
}