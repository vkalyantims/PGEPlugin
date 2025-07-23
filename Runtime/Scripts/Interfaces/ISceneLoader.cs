using System.Collections;

public interface ISceneLoader
{
    /// <summary>
    /// Loads the scene at buildIndex, yielding until it’s done.
    /// </summary>
    IEnumerator LoadScene(int buildIndex);
}
