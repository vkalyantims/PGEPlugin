using EndeaVR.Scripts;
using EndeaVR.SDK.Unity.Models;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerDataProvider: IGetTotalScenes,ITerminateSession
{
    EVRClient _client;
    EndeavrContext EndeavrContext;

    [SerializeField]
    private int numberOfScenarioToBeShown = 15;

    Action<string> restartAppEvent;
    internal void SetRestartAppEvent(Action<string> spawnRestartAppPopup)
    {
        restartAppEvent = spawnRestartAppPopup;
    }
    public Task InitializeEndeavrClientAsync()
    {
        string authToken = "437b930db84b8079c2dd804a71936b5f";
        _client = EVRClient.Instance;

        // Create a TaskCompletionSource to handle the async operation
        var tcs = new TaskCompletionSource<bool>();
        Debug.Log("InitializeEndeavrClientAsync called");

        // Validate the client instance
        if (_client == null)
        {
            restartAppEvent?.Invoke("Client initialization failed. The EVRClient instance is null.");
            tcs.TrySetException(new Exception("EVRClient instance is null."));
            return tcs.Task;
        }

        _client.InitializeWithAuthToken(authToken,
            onSuccess: (context) =>
            {
                if (context == null || context.CurrentSession == null)
                {
                    restartAppEvent?.Invoke("Error: Received null context or session.");
                    tcs.TrySetException(new Exception("Received null context or session."));
                    return;
                }

                Debug.Log($"Session ID: {context.CurrentSession.sessionId}");
                Debug.Log($"Stream URL: {context.CurrentSession.streamUrl}");
                _client.NotifySessionStart();

                var currentContext = context;
                EndeavrContext = context;

                _client.RegisterOnNewSessionEventHandler((newContext) =>
                {
                    if (newContext == null || newContext.CurrentSession == null)
                    {
                        restartAppEvent?.Invoke("Error: Received null session data during session change.");
                        return;
                    }

                    if (newContext.CurrentSession.sessionId != currentContext.CurrentSession.sessionId)
                    {
                        restartAppEvent?.Invoke("This Session is invalid, Please Restart application");
                    }
                    else
                    {
                        EndeavrContext = newContext;
                    }
                });

                _client.GetSessionScenario(
                   onSuccess: (scenario) =>
                   {
                       if (scenario == null)
                       {
                           restartAppEvent?.Invoke("Error: Scenario is null.");
                           tcs.TrySetException(new Exception("Scenario is null."));
                           return;
                       }
                       MapInputVariables(scenario);
                       tcs.TrySetResult(true);
                   },
                   onError: (error) =>
                   {
                       restartAppEvent?.Invoke($"Error retrieving scenario: {error}");
                       tcs.TrySetException(new Exception($"Error retrieving scenario: {error}"));
                       Debug.Log(error);
                   }
                );
            },
            onError: (error) =>
            {
                //restartAppEvent?.Invoke($"Error during initialization: {error}");
                tcs.TrySetException(new Exception($"Error during initialization: {error}"));
            }
        );

        return tcs.Task;
    }
    public void MapInputVariables(Package settings)
    {
        foreach (var item in settings.Settings)
        {
            switch (item.VariableName)
            {
                case "Number Of Scenarios":
                    int parsedValue = int.Parse(item.Value);
                    numberOfScenarioToBeShown = parsedValue;
                    break;
            }
        }
    }
    public int GetNumberOfScenesRequired()
    {
        return numberOfScenarioToBeShown;
    }
    internal string GetStreamURL()
    {
        return EndeavrContext.CurrentSession.streamUrl;
    }

    public void TerminateSession()
    {
        _client.EndSession(
            onSuccess: (scenario) =>
            {
                Debug.Log("Session termainted");
                Debug.Log("Quit Application");
                Application.Quit();
            },
            onError: (error) =>
            {

            }
        );

        Task.Delay(3000).ContinueWith(_ =>
        {
            // Ensure the application quits after a delay to allow any cleanup
            Application.Quit();
        });
    }
}

public interface ITerminateSession
{
    public void TerminateSession();
}