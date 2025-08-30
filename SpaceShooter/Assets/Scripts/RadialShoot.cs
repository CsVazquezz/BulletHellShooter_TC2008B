using UnityEngine;

/// <summary>
/// Circular spiral shooting pattern that creates spiral and circular formations.
/// Uses mathematical functions for Phase 3 boss patterns.
/// </summary>
public class RadialShoot : MonoBehaviour
{
    [Header("Spiral Pattern Settings")]
    public int bulletsPerBurst = 6;    // Drastically reduced from 10
    public float cooldown = 0.8f;      // Much slower firing
    public float speed = 6f;
    public BulletPool bulletPool;

    [Header("Spiral Mathematics")]
    public float spiralTightness = 0.5f; // How tight the spiral is
    public int spiralArms = 2;          // Keep at 2
    public bool clockwise = true;
    public float radiusGrowth = 0.1f; // How fast spiral expands
    
    private float cooldownTime = 0f;
    private float spiralPhase = 0f; // Rotation for animated spirals

    /// <summary>
    /// Updates the cooldown timer and triggers spiral shooting patterns.
    /// Creates animated spiral effects.
    /// </summary>
    void Update()
    {
        if (!enabled) return;
        
        cooldownTime -= Time.deltaTime;
        spiralPhase += Time.deltaTime * 60f * (clockwise ? 1f : -1f); // Animate spiral

        if (cooldownTime <= 0f)
        {
            Debug.Log("RadialShoot: Creating mathematical spiral pattern");
            ShootSpiralPattern();
            cooldownTime += cooldown;
        }
    }

    /// <summary>
    /// Creates spiral pattern using mathematical spiral calculations.
    /// Uses formula: r = a * e^(b*θ) for spiral shape
    /// </summary>
    void ShootSpiralPattern()
    {
        if (bulletPool == null) 
        {
            Debug.LogError("RadialShoot: BulletPool is null!");
            return;
        }
        
        Debug.Log($"RadialShoot: Creating {spiralArms}-armed spiral with {bulletsPerBurst} bullets");
        
        for (int arm = 0; arm < spiralArms; arm++)
        {
            float armOffset = (360f / spiralArms) * arm;
            
            int bulletsPerArm = bulletsPerBurst / spiralArms;
            for (int i = 0; i < bulletsPerArm; i++)
            {
                // Calculate spiral mathematics
                float t = (float)i / bulletsPerArm; // 0 to 1 along spiral
                float angle = (t * 360f * 2f + armOffset + spiralPhase) * Mathf.Deg2Rad; // Multiple rotations
                
                // Spiral equation: r = a * e^(b*θ)
                float radius = 0.5f + radiusGrowth * Mathf.Exp(spiralTightness * t * 3f);
                
                // Convert to x,y coordinates
                Vector3 direction = new Vector3(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius,
                    0f
                ).normalized;
                
                // Vary speed based on spiral position for visual effect
                float speedModifier = 1f + 0.4f * Mathf.Sin(angle * 2f);
                Vector3 velocity = direction * speed * speedModifier;
                
                Shot(transform.position, velocity, 0f);
            }
        }
        
        // Add inner circle bullets for density
        ShootInnerCircle();
    }
    
    /// <summary>
    /// Adds inner circular pattern for visual density.
    /// </summary>
    void ShootInnerCircle()
    {
        int innerBullets = 4; // Reduced from 8
        for (int i = 0; i < innerBullets; i++)
        {
            float angle = (360f / innerBullets) * i + spiralPhase * 0.5f;
            angle *= Mathf.Deg2Rad;
            
            Vector3 direction = new Vector3(
                Mathf.Cos(angle),
                Mathf.Sin(angle),
                0f
            );
            
            Vector3 velocity = direction * speed * 0.8f; // Slower inner bullets
            Shot(transform.position, velocity, 0f);
        }
    }

    /// <summary>
    /// Fires a single bullet with optional curve effect.
    /// </summary>
    void Shot(Vector3 origin, Vector3 velocity, float curve = 0f)
    {
        Bullet bullet = bulletPool.RequestBullet();
        if (bullet != null)
        {
            bullet.transform.position = origin;
            bullet.velocity = velocity;
            
            // Add curve strength if the bullet supports it
            if (bullet.TryGetComponent<BulletCurve>(out var curveComponent))
            {
                curveComponent.curveStrength = curve;
            }
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
