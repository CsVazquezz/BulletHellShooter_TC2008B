using UnityEngine;
using System;

/// <summary>
/// TimeManager handles the in-game time system and triggers events at specific intervals.
/// Used to control boss phases and pattern changes in bullet hell gameplay.
/// </summary>
public class TimeManager : MonoBehaviour
{
    public static int Minute { get; private set; } = 0;
    public static int Second { get; private set; } = 0;
    public static float GameTime { get; private set; } = 0f;
    
    public static event Action OnMinuteChanged;
    public static event Action OnSecondChanged;
    
    [Header("Time Settings")]
    [SerializeField] private float timeScale = 3f; // How fast time passes (3 = 3x speed)
    [SerializeField] private bool autoStart = true;
    
    private bool isRunning = false;
    private int lastMinute = -1;
    private int lastSecond = -1;
    
    void Start()
    {
        if (autoStart)
        {
            StartTimer();
        }
        
        // Reset static values
        Minute = 0;
        Second = 0;
        GameTime = 0f;
    }
    
    void Update()
    {
        if (!isRunning) return;
        
        // Update game time
        GameTime += Time.deltaTime * timeScale;
        
        // Calculate minutes and seconds
        int totalSeconds = Mathf.FloorToInt(GameTime);
        Minute = totalSeconds / 60;
        Second = totalSeconds % 60;
        
        // Trigger events when time changes
        if (Second != lastSecond)
        {
            lastSecond = Second;
            OnSecondChanged?.Invoke();
            
            // Debug every 5 seconds
            if (Second % 5 == 0)
            {
                Debug.Log($"TimeManager: Current time {GetFormattedTime()}");
            }
        }
        
        if (Minute != lastMinute)
        {
            lastMinute = Minute;
            OnMinuteChanged?.Invoke();
            Debug.Log($"TimeManager: Minute {Minute} reached! Triggering OnMinuteChanged event.");
        }
    }
    
    public void StartTimer()
    {
        isRunning = true;
        Debug.Log("Time Manager: Timer started");
    }
    
    public void StopTimer()
    {
        isRunning = false;
        Debug.Log("Time Manager: Timer stopped");
    }
    
    public void ResetTimer()
    {
        GameTime = 0f;
        Minute = 0;
        Second = 0;
        lastMinute = -1;
        lastSecond = -1;
        Debug.Log("Time Manager: Timer reset");
    }
    
    /// <summary>
    /// Get formatted time string
    /// </summary>
    public static string GetFormattedTime()
    {
        return $"{Minute:D2}:{Second:D2}";
    }
}
