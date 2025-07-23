using System;

public class Timer
{
    private float startTime;
    private bool isRunning;

    // Starts or restarts the timer
    public void Start()
    {
        startTime = UnityEngine.Time.time;
        isRunning = true;
    }

    // Stops the timer and returns elapsed time in seconds
    public float Stop()
    {
        if (!isRunning)
            throw new InvalidOperationException("Timer is not running.");

        isRunning = false;
        return UnityEngine.Time.time - startTime;
    }
}
