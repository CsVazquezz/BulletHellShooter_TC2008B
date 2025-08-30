using UnityEngine;

/// <summary>
/// Star pattern shooting that creates beautiful star-shaped bullet formations.
/// Uses mathematical functions to create varying bullet speeds for star effect.
/// </summary>
public class StarShoot : MonoBehaviour
{
    [Header("Star Settings")]
    public int bullets = 6;         // Drastically reduced from 12
    public float cooldown = 0.8f;   // Much slower firing
    public float speed = 7f;
    public BulletPool bulletPool;
    
    [Header("Star Shape")]
    public int starPoints = 5;
    public float speedVariation = 0.8f; // How much speed varies (0-1)
    
    private float cooldownTime = 0f;

    /// <summary>
    /// Updates the cooldown timer and triggers shooting when the cooldown reaches zero.
    /// </summary>
    void Update()
    {
        if (!enabled) return;
        
        cooldownTime -= Time.deltaTime;

        if (cooldownTime <= 0f)
        {
            StarShot(transform.position, transform.up, bullets, speed);
            cooldownTime += cooldown;
        }
    }

    /// <summary>
    /// Fires multiple bullets arranged in a true mathematical star pattern.
    /// Creates pointed star shape with inner/outer radius variations.
    /// </summary>
    void StarShot(Vector3 origin, Vector3 direction, int bulletCount, float baseSpeed)
    {
        if (bulletPool == null) return;
        
        Debug.Log($"StarShoot: Creating mathematical star with {starPoints} points");
        
        // Create star pattern with mathematical precision
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = (360f / bulletCount) * i * Mathf.Deg2Rad;
            
            // Create star shape using coordinates
            // r = a + b*cos(k*θ) where k = starPoints for star shape
            float starRadius = 1f + 0.6f * Mathf.Cos(starPoints * angle);
            
            // Calculate direction with star shape influence
            Vector3 bulletDirection = new Vector3(
                Mathf.Cos(angle) * starRadius,
                Mathf.Sin(angle) * starRadius,
                0f
            ).normalized;

            // Vary speed based on star points for dramatic effect
            float speedModifier = 1f + 0.4f * Mathf.Cos(starPoints * angle);
            float bulletSpeed = baseSpeed * speedModifier;

            Shot(origin, bulletDirection * bulletSpeed);
        }
    }

    /// <summary>
    /// Fires a single bullet.
    /// </summary>
    void Shot(Vector3 origin, Vector3 velocity)
    {
        Bullet bullet = bulletPool.RequestBullet();
        if (bullet != null)
        {
            bullet.transform.position = origin;
            bullet.velocity = velocity;
        }
    }

    /// <summary>
    /// Rotates a vector by a given angle in degrees around the Z-axis.
    /// </summary>
    Vector2 RotateVector(Vector2 originalVector, float rotateAngle)
    {
        Quaternion rotation = Quaternion.AngleAxis(rotateAngle, Vector3.forward);
        return rotation * originalVector;
    }
    
    /// <summary>
    /// Enable this shooting component
    /// </summary>
    public void EnableShooting()
    {
        enabled = true;
        cooldownTime = 0f;
    }
    
    /// <summary>
    /// Disable this shooting component
    /// </summary>
    public void DisableShooting()
    {
        enabled = false;
    }
}
