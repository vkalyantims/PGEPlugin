using EndeaVR.Scripts;
using System.Collections;
using UnityEngine;

public class SessionObserver : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(InitializeWatcher());
    }

    private IEnumerator InitializeWatcher()
    {
        // wait here until EVRClient.Instance != null
        yield return new WaitUntil(() => EVRClient.Instance != null);

        // once we have an instance, fire SessionWatcher exactly once
        StartCoroutine(SessionWatcher());
    }
    private IEnumerator SessionWatcher()
    {
        while (true)
        {
            Debug.Log("Session watched running!");
            bool done = false;
            bool gotSession = false;
            string errorMsg = null;
            EVRClient.Instance.GetSession(
                onSuccess: session =>
                {
                    gotSession = session != null;
                    done = true;
                },
                onError: err =>
                {
                    errorMsg = err;
                    done = true;
                }
            );

            // wait until one of the callbacks flips `done`
            yield return new WaitUntil(() => done);

            // if we never got a live session, bail out
            if (!gotSession)
            {
                Debug.LogError($"Session failed: {errorMsg}");
                TerminateSession();
                yield break;
            }

            // wait before trying again
            yield return new WaitForSeconds(5);
        }
    }
    internal void TerminateSession()
    {
        EVRClient.Instance.EndSession(
            onSuccess: (msg) =>
            {
                Debug.Log("Session terminated");
            },
            onError: (error) =>
            {
                Debug.Log("Endsession: " + error);
            }
        );
        Debug.Log("Quitting App");
        Application.Quit();
    }
}
