using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : ISceneLoader
{
    public IEnumerator LoadScene(int buildIndex)
    {
        var sceneProgress = SceneManager.LoadSceneAsync(buildIndex);
        while (!sceneProgress.isDone)
        {
            Debug.Log("Scene loading");
            yield return null;
        }
    }
}