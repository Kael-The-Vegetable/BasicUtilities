using BasicUtilities;
using UnityEngine;

public class TestingTimers : MonoBehaviour
{
    public StopwatchTimer stopwatch;
    public CountdownTimer countdown;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countdown = new(2f);
        countdown.OnStop += (CountdownTimer timer) => countdown.Restart();
        countdown.OnStop += (CountdownTimer timer) => Debug.Log("2 seconds passed");
        countdown.Start();

        stopwatch = new();
        stopwatch.OnStop += (StopwatchTimer timer) => Debug.Log("Stopped");
        stopwatch.OnPause += (StopwatchTimer timer) => Debug.Log("Paused");
        stopwatch.OnResume += (StopwatchTimer timer) => Debug.Log("Resumed");
        stopwatch.OnLap += (StopwatchTimer timer) =>
        {
            for (int i = 0; i < timer.Laps.Count; i++)
            {
                Debug.Log($"Lap {i} : {timer.Laps[i]}");
            }
            Debug.Log($"Total Elapsed : {timer.ElapsedTime}");
        };
		stopwatch.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stopwatch.Pause(!stopwatch.IsPaused);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            stopwatch.Lap();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OneShotTimer.Delay(2, () => Debug.Log("C Pressed"), destroyCancellationToken);
        }
    }
}
