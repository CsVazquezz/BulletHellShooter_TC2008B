using UnityEngine;

/// <summary>
/// Wave pattern shooting that creates sine wave formations.
/// Mathematical wave patterns for Phase 2 of boss fight.
/// </summary>
public class LinearShoot : MonoBehaviour
{
    [Header("Wave Pattern Settings")]
    public float cooldown = 1.0f;      // Much slower firing
    public float speed = 8f;
    public BulletPool bulletPool;
    
    [Header("Wave Mathematics")]
    public int bulletsPerWave = 3;      // Drastically reduced from 5
    public float waveWidth = 6f; // Width of the wave
    public float waveFrequency = 2f; // How many waves across the width
    public float waveAmplitude = 1f; // Height of the wave curves
    
    private float cooldownTime = 0f;
    private float wavePhase = 0f; // Phase shift for animated waves
    
    /// <summary>
    /// Updates the cooldown timer and triggers shooting when the cooldown reaches zero.
    /// Creates animated sine wave patterns.
    /// </summary>
    void Update()
    {
        if (!enabled) return;
        
        cooldownTime -= Time.deltaTime;
        wavePhase += Time.deltaTime * 3f; // Animate the wave

        if (cooldownTime <= 0f)
        {
            Debug.Log("LinearShoot: Creating mathematical wave pattern");
            ShootWavePattern();
            cooldownTime += cooldown;
        }
    }

    /// <summary>
    /// Creates wave pattern using sine function mathematics.
    /// Each bullet follows a sine wave trajectory.
    /// </summary>
    void ShootWavePattern()
    {
        if (bulletPool == null) 
        {
            Debug.LogError("LinearShoot: BulletPool is null!");
            return;
        }
        
        Debug.Log($"LinearShoot: Creating wave with {bulletsPerWave} bullets");
        
        for (int i = 0; i < bulletsPerWave; i++)
        {
            // Calculate position across wave width
            float t = (float)i / (bulletsPerWave - 1); // 0 to 1 across the wave
            float x = (t - 0.5f) * waveWidth; // Center the wave
            
            // Calculate sine wave mathematics: y = A * sin(f * x + phase)
            float sineInput = waveFrequency * Mathf.PI * t + wavePhase;
            float y = waveAmplitude * Mathf.Sin(sineInput);
            
            // Create wave direction (combines downward movement with sine curve)
            Vector3 waveDirection = new Vector3(
                x * 0.3f, // Horizontal component for wave shape
                -1f + y * 0.2f, // Mainly downward with wave curve
                0f
            ).normalized;
            
            // Vary speed based on wave position for visual effect
            float speedModifier = 1f + 0.3f * Mathf.Sin(sineInput);
            Vector3 velocity = waveDirection * speed * speedModifier;
            
            // Offset starting position slightly for wave effect
            Vector3 startPosition = transform.position + new Vector3(x * 0.1f, 0f, 0f);
            
            Shot(startPosition, velocity);
        }
    }

    /// <summary>
    /// Fires a single bullet in the specified direction.
    /// </summary>
    void Shot(Vector3 origin, Vector3 velocity)
    {
        if (bulletPool == null) 
        {
            Debug.LogError("LinearShoot: BulletPool is null!");
            return;
        }
        
        Bullet bullet = bulletPool.RequestBullet();
        if (bullet != null)
        {
            bullet.transform.position = origin;
            bullet.velocity = velocity;
            Debug.Log($"LinearShoot: Bullet fired from {origin} with velocity {velocity}");
        }
        else
        {
            Debug.LogWarning("LinearShoot: Failed to get bullet from pool!");
        }
    }
    
    /// <summary>
    /// Enable this shooting component
    /// </summary>
    public void EnableShooting()
    {
        enabled = true;
        cooldownTime = 0f; // Fire immediately when enabled
    }
    
    /// <summary>
    /// Disable this shooting component
    /// </summary>
    public void DisableShooting()
    {
        enabled = false;
    }
}
