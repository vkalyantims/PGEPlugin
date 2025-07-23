using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    [Tooltip("How often to update the on-screen FPS text (seconds)")]
    public float uiUpdateInterval = 0.5f;

    [Tooltip("How strongly to smooth the FPS (0 = no smoothing, 1 = infinite smoothing)")]
    [Range(0f, 1f)]
    public float smoothingFactor = 0.1f;

    [Tooltip("How often to log min/max/avg FPS (seconds)")]
    public float logInterval = 10f;

    [SerializeField]
    private GameObject fpsView;
    float _uiTimer;
    float _logTimer;
    float _displayedFPS;   // smoothed value used for UI

    // stats over logInterval
    float _sumFPS;
    float _minFPS = float.MaxValue;
    float _maxFPS = float.MinValue;
    int _frameCount;

    [SerializeField]
    private ISaveInformation saveInformation;
    private string sceneName;
    public void SetSaveInfo(ISaveInformation saveInfo, string sceneName)
    {
        Debug.Log("SaveInfo Assigned");
        saveInformation = saveInfo;
        this.sceneName = sceneName;
    }
    public void Enable()
    {
        fpsView.SetActive(true);
    }
    public void Disable()
    {
        fpsView.SetActive(false);
    }
    void Update()
    {
        // 1) Instant FPS
        float instFPS = 1f / Time.unscaledDeltaTime;

        // 2) Smooth it
        _displayedFPS = Mathf.Lerp(_displayedFPS, instFPS, 1f - smoothingFactor);

        // 3) UI throttle
        _uiTimer += Time.unscaledDeltaTime;
        if (_uiTimer >= uiUpdateInterval)
        {
            if (fpsText != null)
                fpsText.text = $"{_displayedFPS:0.}";
            _uiTimer = 0f;
        }

        // 4) Accumulate for stats
        _logTimer += Time.unscaledDeltaTime;
        _sumFPS += instFPS;
        _frameCount++;
        _minFPS = Mathf.Min(_minFPS, instFPS);
        _maxFPS = Mathf.Max(_maxFPS, instFPS);

        // 5) Log every logInterval
        if (_logTimer >= logInterval)
        {
            float avgFPS = _sumFPS / _frameCount;
            Debug.Log($"Last {_logTimer:0.0}s — Min: {_minFPS:0.}, Max: {_maxFPS:0.}, Avg: {avgFPS:0.}");

            if(saveInformation != null)
            {
                Debug.Log("FPS Saved");
                saveInformation.SaveData($"Scenario - {sceneName} - Min: {_minFPS:0.}, Max: {_maxFPS:0.}", $"Avg: {avgFPS:0.}");
            }
            _logTimer = 0f;
            _sumFPS = 0f;
            _frameCount = 0;
            _minFPS = float.MaxValue;
            _maxFPS = float.MinValue;
        }
    }
}
