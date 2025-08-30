using UnityEngine;
using TMPro;

/// <summary>
/// Real-time bullet counter that shows active bullets on screen.
/// Updates immediately when bullets are created or destroyed.
/// Helps monitor performance in bullet hell scenarios.
/// </summary>
public class BulletCounter : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI counterText;
    
    [Header("Display Settings")]
    public string displayFormat = "Active Bullets: {0}";
    public Color normalColor = Color.white;
    public Color warningColor = Color.yellow;
    public Color dangerColor = Color.red;
    
    [Header("Performance Thresholds")]
    public int warningThreshold = 150;
    public int dangerThreshold = 300;
    
    // Static instance for easy access
    public static BulletCounter Instance;
    
    private int activeBulletCount = 0;
    
    void Awake()
    {
        // Singleton pattern for easy access
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep alive across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Auto-find TextMeshPro if not assigned
        if (counterText == null)
        {
            counterText = GetComponent<TextMeshProUGUI>();
        }
        
        // Reset counter and update display
        activeBulletCount = 0;
        UpdateDisplay();
        
        Debug.Log("BulletCounter initialized and ready!");
    }
    
    /// <summary>
    /// Call this when a bullet becomes active (fired/enabled)
    /// </summary>
    public void IncrementBulletCount()
    {
        activeBulletCount++;
        Debug.Log($"BulletCounter: +1 bullet (Total: {activeBulletCount})");
        UpdateDisplay();
    }
    
    /// <summary>
    /// Call this when a bullet becomes inactive (destroyed/pooled)
    /// </summary>
    public void DecrementBulletCount()
    {
        activeBulletCount--;
        if (activeBulletCount < 0) activeBulletCount = 0; // Safety check
        Debug.Log($"BulletCounter: -1 bullet (Total: {activeBulletCount})");
        UpdateDisplay();
    }
    
    /// <summary>
    /// Reset counter to zero
    /// </summary>
    public void ResetCounter()
    {
        activeBulletCount = 0;
        UpdateDisplay();
    }
    
    /// <summary>
    /// Get current active bullet count
    /// </summary>
    public int GetActiveBulletCount()
    {
        return activeBulletCount;
    }
    
    /// <summary>
    /// Updates the UI display with current count and performance color
    /// </summary>
    void UpdateDisplay()
    {
        if (counterText == null) return;
        
        // Update text
        counterText.text = string.Format(displayFormat, activeBulletCount);
        
        // Update color based on performance thresholds
        if (activeBulletCount >= dangerThreshold)
        {
            counterText.color = dangerColor;
        }
        else if (activeBulletCount >= warningThreshold)
        {
            counterText.color = warningColor;
        }
        else
        {
            counterText.color = normalColor;
        }
    }
    
    /// <summary>
    /// Manual count verification (for debugging)
    /// </summary>
    [ContextMenu("Verify Bullet Count")]
    public void VerifyBulletCount()
    {
        Bullet[] activeBullets = FindObjectsByType<Bullet>(FindObjectsSortMode.None);
        int actualCount = 0;
        
        foreach (Bullet bullet in activeBullets)
        {
            if (bullet.gameObject.activeInHierarchy)
            {
                actualCount++;
            }
        }
        
        Debug.Log($"Counter shows: {activeBulletCount}, Actual active bullets: {actualCount}");
        
        if (activeBulletCount != actualCount)
        {
            Debug.LogWarning("Counter mismatch detected! Correcting...");
            activeBulletCount = actualCount;
            UpdateDisplay();
        }
    }
}
