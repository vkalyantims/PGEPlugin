using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Threading;

public class ConnectionHandler : MonoBehaviour, IConnectionHandler
{
    public event Action<IGetTotalScenes,bool, ITerminateSession> OnConnected;
    public event Action<Exception> OnFailed;

    [SerializeField] private ConnectingScreen connectingScreenPrefab;
    bool isConnectionEstablished = false;
    ServerDataProvider provider;
    [SerializeField]
    private RestartAppHandler restartApp;
    private ConnectingScreen screen;
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);
        await InitializeConnectionAsync();
        Camera.onPreRender += HandleCameraPreRender;
    }

    private void HandleCameraPreRender(Camera cam)
    {
        Camera.onPreRender -= HandleCameraPreRender;
        if (screen != null)
        {
            screen.gameObject.GetComponent<RecenterController>().RecenterSelf();
        }
    }
    private async Task InitializeConnectionAsync()
    {
        screen = Instantiate(connectingScreenPrefab);
        screen.SetConnectionStatus("Connecting to the server…");
        await Task.Delay(3000); // Simulate some delay for the connection attempt

        screen.SetConnectionStatus("Initializing Endeavr SDK…");

        provider = new ServerDataProvider();
        provider.SetRestartAppEvent(SpawnRestartAppPopup);

        using var cts = new CancellationTokenSource();
        var timeout = Task.Delay(TimeSpan.FromSeconds(8), cts.Token);
        var initTask = provider.InitializeEndeavrClientAsync();

        var completed = await Task.WhenAny(initTask, timeout);
        if (completed == timeout)
        {
            screen.SetConnectionStatus("Connection timed out. Proceeding anyway…");
            await Task.Delay(3000); // Simulate some delay for the connection attempt
        }
        else
        {
            cts.Cancel(); // cancel the timeout
            try
            {
                await initTask; // observe exceptions
                screen.SetConnectionStatus("Connected! Preparing the scenario…");
                isConnectionEstablished = true;
            }
            catch (Exception ex)
            {
                screen.SetConnectionStatus($"Connection failed: {ex.Message}");
                OnFailed?.Invoke(ex);
                await Task.Delay(2000);
            }
        }

        screen.gameObject.SetActive(false);
        OnConnected?.Invoke(provider, isConnectionEstablished, provider);
    }

    private void SpawnRestartAppPopup(string msg)
    {
        RestartAppHandler spawnedRestartUI = Instantiate(restartApp.gameObject).GetComponent<RestartAppHandler>();
        spawnedRestartUI.SetMessage(msg);
        spawnedRestartUI.transform.SetParent(Camera.main.transform, false);
        spawnedRestartUI.AddTerminateSessionEvent(()=>Application.Quit());
        spawnedRestartUI.FadeOut();
    }

}

