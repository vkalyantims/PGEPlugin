using PGE.Diagnosis;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
[CreateAssetMenu(fileName = "SceneData", menuName = "Scene Management/Scene Data")]
public class SceneData : ScriptableObject
{

    public SceneInformation sceneInformation;


#if UNITY_EDITOR
    [SerializeField] private SceneAsset sceneAsset; // Assign scene in Editor
#endif
    [SerializeField] private int sceneIndex;
    public int SceneIndex => sceneIndex; // Public getter
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            sceneIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
        }
    }
#endif

    public List<QuestionRecord> QuestionRecords;
    
}
