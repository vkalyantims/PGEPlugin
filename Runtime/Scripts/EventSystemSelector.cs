using UnityEngine;

public class EventSystemSelector : MonoBehaviour
{
    [Header("Event System Prefabs")]
    public GameObject editorEventSystemPrefab;
    public GameObject apkEventSystemPrefab;

    private static bool eventSystemInstantiated = false;

    void Awake()
    {
        if (eventSystemInstantiated || FindObjectOfType<UnityEngine.EventSystems.EventSystem>() != null)
            return;

        GameObject eventSystemInstance = null;

#if UNITY_EDITOR
        if (editorEventSystemPrefab != null)
        {
            eventSystemInstance = Instantiate(editorEventSystemPrefab);
            Debug.Log("Editor Event System instantiated.");
        }
#else
        if (apkEventSystemPrefab != null)
        {
            eventSystemInstance = Instantiate(apkEventSystemPrefab);
            Debug.Log("APK Event System instantiated.");
        }
#endif

        if (eventSystemInstance != null)
        {
            DontDestroyOnLoad(eventSystemInstance);
            eventSystemInstantiated = true;
        }
    }
}
