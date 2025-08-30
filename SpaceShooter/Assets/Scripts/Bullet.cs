using UnityEngine;

/// <summary>
/// Basic bullet script that moves forward and handles collision detection.
/// Uses object pooling instead of Destroy() for better performance.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float maxTime = 3.5f;
    
    private float actualTime = 0f;
    public Vector3 velocity;
    private BulletPool bulletPool;
    
    private void OnEnable()
    {
        actualTime = 0f;
        Debug.Log($"Bullet enabled at position {transform.position} with scale {transform.localScale}");
        
        // Increment bullet counter when bullet becomes active
        if (BulletCounter.Instance != null)
        {
            BulletCounter.Instance.IncrementBulletCount();
        }
        
        // Check if sprite renderer exists and has a sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            if (sr.sprite != null)
            {
                Debug.Log($"Bullet has sprite: {sr.sprite.name}, color: {sr.color}");
            }
            else
            {
                Debug.LogWarning("Bullet SpriteRenderer has no sprite assigned!");
            }
        }
        else
        {
            Debug.LogWarning("Bullet has no SpriteRenderer component!");
        }
    }
    
    /// <summary>
    /// Updates the bullet's position every frame and disables it if maxTime is exceeded.
    /// </summary>
    private void Update()
    {
        float dt = Time.deltaTime;

        // Move bullet using transform (more reliable than Rigidbody2D)
        transform.position += velocity * dt;
        actualTime += dt;

        // Debug bullet position every 60 frames (about 1 second)
        if (Time.frameCount % 60 == 0)
        {
            Debug.Log($"Bullet at position {transform.position}, velocity {velocity}, time {actualTime:F1}s");
        }

        if (actualTime > maxTime)
            DeactivateBullet();
    }
    
    /// <summary>
    /// Handle collision detection - deactivate bullet when hitting something
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore collision with the turret that fired it
        if (other.CompareTag("Turret"))
            return;
            
        // Check for targets or boundaries
        if (other.CompareTag("Enemy") || other.CompareTag("Player") || other.CompareTag("Boundary") || other.CompareTag("Wall"))
        {
            
            DeactivateBullet();
        }
    }
    
    /// <summary>
    /// Return bullet to pool instead of destroying it
    /// </summary>
    private void DeactivateBullet()
    {
        // Decrement bullet counter when bullet becomes inactive
        if (BulletCounter.Instance != null)
        {
            BulletCounter.Instance.DecrementBulletCount();
        }
        
        actualTime = 0f;
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Safety method - only decrement if not already handled by DeactivateBullet
    /// </summary>
    private void OnDisable()
    {
        // This handles cases where SetActive(false) is called directly
        // but we need to avoid double-counting with DeactivateBullet()
        if (actualTime > 0f && BulletCounter.Instance != null)
        {
            BulletCounter.Instance.DecrementBulletCount();
            actualTime = 0f; // Reset to prevent double-counting
        }
    }
    
    /// <summary>
    /// Set the bullet pool reference for this bullet
    /// </summary>
    public void SetBulletPool(BulletPool pool)
    {
        bulletPool = pool;
    }
}
