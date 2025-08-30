using System.Collections;
using UnityEngine;

/// <summary>
/// Main boss controller that orchestrates different attack patterns based on time.
/// Creates an epic bullet hell experience with multiple phases.
/// </summary>
public class BossController : MonoBehaviour
{
    [Header("Shooting Components")]
    public LinearShoot linearShoot;
    public RadialShoot radialShoot;
    public StarShoot starShoot;
    
    [Header("Movement Settings")]
    public float movementSpeed = 2f;
    public Vector3[] cornerPositions = {
        new Vector3(6f, 3f, 0f),    // Top right
        new Vector3(-6f, 3f, 0f),   // Top left
        new Vector3(-6f, -1f, 0f),  // Bottom left
        new Vector3(6f, -1f, 0f)    // Bottom right
    };
    
    [Header("Phase Timing - 10 Second Intervals")]
    public int phase1StartTime = 0;   // Phase 1: STAR PATTERNS (0-10 seconds)
    public int phase2StartTime = 10;  // Phase 2: WAVE PATTERNS (10-20 seconds)  
    public int phase3StartTime = 20;  // Phase 3: SPIRAL PATTERNS (20+ seconds)
    
    private bool phase1Started = false;
    private bool phase2Started = false;
    private bool phase3Started = false;
    
    void OnEnable()
    {
        TimeManager.OnSecondChanged += CheckPhaseTransitions;
    }
    
    void OnDisable()
    {
        TimeManager.OnSecondChanged -= CheckPhaseTransitions;
    }
    
    void Start()
    {
        // Disable all shooting patterns initially
        if (linearShoot != null) linearShoot.DisableShooting();
        if (radialShoot != null) radialShoot.DisableShooting();
        if (starShoot != null) starShoot.DisableShooting();
        
        // Position boss at center
        transform.position = Vector3.zero;
        
        Debug.Log("Boss Controller initialized. Waiting for phase triggers...");
        
        // Manual trigger for testing - start Phase 1 immediately
        Invoke("StartPhase1Manual", 2f); // Start after 2 seconds for testing
    }
    
    /// <summary>
    /// Manual trigger for testing Phase 1
    /// </summary>
    void StartPhase1Manual()
    {
        if (!phase1Started)
        {
            Debug.Log("BossController: Manual Phase 1 trigger!");
            StartCoroutine(Phase1Pattern());
        }
    }
    
    /// <summary>
    /// Check if it's time to start a new phase based on seconds (with cycling)
    /// </summary>
    void CheckPhaseTransitions()
    {
        int currentSecond = TimeManager.Second;
        int totalSeconds = TimeManager.Minute * 60 + currentSecond;
        
        // Create 30-second cycles (each cycle: 0-10s, 10-20s, 20-30s, then repeat)
        int cycleTime = totalSeconds % 30;
        
        Debug.Log($"BossController: Checking transitions at {totalSeconds}s (cycle: {cycleTime}s)");
        
        // Phase 1: 0-10 seconds in each cycle
        if (cycleTime >= 0 && cycleTime < 10 && !phase1Started)
        {
            ResetAllPhases();
            Debug.Log($"BossController: Starting Phase 1 at cycle {cycleTime}s!");
            StartCoroutine(Phase1Pattern());
        }
        // Phase 2: 10-20 seconds in each cycle
        else if (cycleTime >= 10 && cycleTime < 20 && !phase2Started)
        {
            ResetAllPhases();
            Debug.Log($"BossController: Starting Phase 2 at cycle {cycleTime}s!");
            StartCoroutine(Phase2Pattern());
        }
        // Phase 3: 20-30 seconds in each cycle
        else if (cycleTime >= 20 && cycleTime < 30 && !phase3Started)
        {
            ResetAllPhases();
            Debug.Log($"BossController: Starting Phase 3 at cycle {cycleTime}s!");
            StartCoroutine(Phase3Pattern());
        }
    }
    
    /// <summary>
    /// Reset all phases and disable shooting
    /// </summary>
    void ResetAllPhases()
    {
        // Stop all current patterns
        if (linearShoot != null) linearShoot.DisableShooting();
        if (radialShoot != null) radialShoot.DisableShooting();
        if (starShoot != null) starShoot.DisableShooting();
        
        // Reset phase flags for new cycle
        phase1Started = false;
        phase2Started = false;
        phase3Started = false;
    }
    
    /// <summary>
    /// Phase 1: Mathematical STAR PATTERNS - runs for 10 seconds
    /// </summary>
    IEnumerator Phase1Pattern()
    {
        phase1Started = true;
        Debug.Log("Boss Phase 1: MATHEMATICAL STAR PATTERNS Started! (10 seconds)");
        
        // Enable star shooting for mathematical star formations
        if (starShoot != null) starShoot.EnableShooting();
        
        // Run star patterns for exactly 10 seconds
        float phaseTimer = 10f;
        while (phaseTimer > 0f)
        {
            // Move around while shooting stars
            Vector3 randomPosition = new Vector3(
                Random.Range(-4f, 4f),
                Random.Range(1f, 3f),
                0f
            );
            yield return StartCoroutine(MoveTo(randomPosition, 1f));
            
            phaseTimer -= 1f;
            yield return new WaitForSeconds(0.1f);
        }
        
        Debug.Log("Phase 1 Complete - Star patterns finished!");
    }
    
    /// <summary>
    /// Phase 2: Mathematical WAVE PATTERNS - runs for 10 seconds
    /// </summary>
    IEnumerator Phase2Pattern()
    {
        phase2Started = true;
        Debug.Log("Boss Phase 2: MATHEMATICAL WAVE PATTERNS Started! (10 seconds)");
        
        // Enable linear shooting for wave formations
        if (linearShoot != null) linearShoot.EnableShooting();
        
        // Run wave patterns for exactly 10 seconds
        float phaseTimer = 10f;
        while (phaseTimer > 0f)
        {
            // Move horizontally for wave effect
            Vector3 leftPos = new Vector3(-5f, 2f, 0f);
            Vector3 rightPos = new Vector3(5f, 2f, 0f);
            
            yield return StartCoroutine(MoveTo(leftPos, 1.5f));
            yield return StartCoroutine(MoveTo(rightPos, 1.5f));
            
            phaseTimer -= 3f;
            yield return new WaitForSeconds(0.1f);
        }
        
        Debug.Log("Phase 2 Complete - Wave patterns finished!");
    }
    
    /// <summary>
    /// Phase 3: Mathematical SPIRAL PATTERNS - runs for 10 seconds
    /// </summary>
    IEnumerator Phase3Pattern()
    {
        phase3Started = true;
        Debug.Log("Boss Phase 3: MATHEMATICAL SPIRAL PATTERNS Started! (10 seconds)");
        
        // Move to center for spiral patterns
        yield return StartCoroutine(MoveTo(Vector3.zero, 1f));
        
        // Enable radial shooting for spiral formations
        if (radialShoot != null) radialShoot.EnableShooting();
        
        // Run spiral patterns for exactly 10 seconds
        float phaseTimer = 10f;
        while (phaseTimer > 0f)
        {
            // Continuous rotation for spiral effect
            yield return StartCoroutine(RotateBy(180f, 1f));
            phaseTimer -= 1f;
            yield return new WaitForSeconds(0.1f);
        }
        
        // Disable spiral shooting
        if (radialShoot != null) radialShoot.DisableShooting();
        
        Debug.Log("Phase 3 Complete - Waiting for next cycle!");
    }
    
    /// <summary>
    /// Smoothly move to target position
    /// </summary>
    IEnumerator MoveTo(Vector3 targetPos, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }
    
    /// <summary>
    /// Rotate by specified angle over duration
    /// </summary>
    IEnumerator RotateBy(float angle, float duration)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, angle);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
