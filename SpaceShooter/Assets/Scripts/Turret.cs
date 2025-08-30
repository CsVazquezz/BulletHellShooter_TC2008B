using UnityEngine;

/// <summary>
/// Automatic turret that shoots bullets at a fixed fire rate.
/// Does not require a player GameObject to exist in the scene.
/// </summary>
public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    [SerializeField] private float fireRate = 2f; // Bullets per second
    [SerializeField] private Transform barrel; // Where bullets spawn from
    [SerializeField] private BulletPool bulletPool; // Reference to bullet pool
    
    [Header("Shooting Direction")]
    [SerializeField] private Vector2 shootDirection = Vector2.down; // Default shooting direction
    [SerializeField] private bool normalizeDirection = true; // Automatically normalize direction
    
    [Header("Rotation Settings")]
    [SerializeField] private bool rotateTowardsDirection = true; // Rotate turret to face shooting direction
    [SerializeField] private float rotationOffset = 0f; // Additional rotation offset in degrees
    
    [Header("Auto-Fire")]
    [SerializeField] private bool autoFire = true; // Start firing automatically
    [SerializeField] private float startDelay = 0f; // Delay before starting to fire
    
    private float fireTimer;
    private float fireInterval;
    private bool isFiring = false;
    
    void Start()
    {
        // Calculate fire interval from fire rate
        UpdateFireInterval();
        
        // Auto-find bullet pool if not assigned
        if (bulletPool == null)
        {
            bulletPool = FindFirstObjectByType<BulletPool>();
            if (bulletPool == null)
            {
                Debug.LogError($"No BulletPool found in scene! Please assign one to {gameObject.name} or create a BulletPool GameObject.");
            }
            else
            {
                Debug.Log($"BulletPool automatically found and assigned to {gameObject.name}");
            }
        }
        
        // Use transform as barrel if none assigned
        if (barrel == null)
        {
            barrel = transform;
            Debug.Log($"No barrel assigned to {gameObject.name}, using turret transform as barrel position.");
        }
        
        // Normalize shooting direction
        if (normalizeDirection && shootDirection != Vector2.zero)
        {
            shootDirection = shootDirection.normalized;
        }
        
        // Rotate turret to face shooting direction
        if (rotateTowardsDirection)
        {
            RotateTowardsDirection();
        }
        
        // Start auto-firing if enabled
        if (autoFire)
        {
            if (startDelay > 0f)
            {
                Invoke(nameof(StartFiring), startDelay);
            }
            else
            {
                StartFiring();
            }
        }
    }
    
    void Update()
    {
        // Handle automatic firing
        if (isFiring && bulletPool != null)
        {
            fireTimer += Time.deltaTime;
            
            if (fireTimer >= fireInterval)
            {
                Fire();
                fireTimer = 0f;
            }
        }
        
        // Debug information
        if (Time.frameCount % 60 == 0) // Log every 60 frames (about once per second)
        {
            Debug.Log($"Turret Status - isFiring: {isFiring}, bulletPool: {(bulletPool != null ? "Assigned" : "NULL")}, barrel: {(barrel != null ? "Assigned" : "NULL")}");
        }
        
        // Update fire interval if fire rate changed in inspector
        if (Application.isEditor)
        {
            UpdateFireInterval();
        }
    }
    
    /// <summary>
    /// Start the turret firing
    /// </summary>
    public void StartFiring()
    {
        isFiring = true;
        fireTimer = 0f;
        Debug.Log($"{gameObject.name} started firing");
    }
    
    /// <summary>
    /// Stop the turret from firing
    /// </summary>
    public void StopFiring()
    {
        isFiring = false;
        Debug.Log($"{gameObject.name} stopped firing");
    }
    
    /// <summary>
    /// Fire a single bullet
    /// </summary>
    public void Fire()
    {
        if (bulletPool == null)
        {
            Debug.LogWarning($"{gameObject.name} cannot fire: BulletPool is not assigned!");
            return;
        }
        
        if (barrel == null)
        {
            Debug.LogWarning($"{gameObject.name} cannot fire: Barrel is not assigned!");
            return;
        }
        
        Vector3 spawnPosition = barrel.position;
        Bullet bullet = bulletPool.ShootBullet(spawnPosition, shootDirection, 10f);
        
        if (bullet != null)
        {
            Debug.Log($"{gameObject.name} fired bullet at {spawnPosition} in direction {shootDirection}");
        }
    }
    
    /// <summary>
    /// Update the shooting direction
    /// </summary>
    /// <param name="newDirection">New direction to shoot</param>
    public void SetShootDirection(Vector2 newDirection)
    {
        shootDirection = normalizeDirection ? newDirection.normalized : newDirection;
        
        if (rotateTowardsDirection)
        {
            RotateTowardsDirection();
        }
    }
    
    /// <summary>
    /// Update the fire rate
    /// </summary>
    /// <param name="newFireRate">New fire rate in bullets per second</param>
    public void SetFireRate(float newFireRate)
    {
        fireRate = Mathf.Max(0.1f, newFireRate); // Minimum fire rate of 0.1
        UpdateFireInterval();
    }
    
    /// <summary>
    /// Calculate fire interval from fire rate
    /// </summary>
    private void UpdateFireInterval()
    {
        fireInterval = fireRate > 0 ? 1f / fireRate : 1f;
    }
    
    /// <summary>
    /// Rotate turret to face the shooting direction
    /// </summary>
    private void RotateTowardsDirection()
    {
        if (shootDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + rotationOffset, Vector3.forward);
        }
    }
    
    /// <summary>
    /// Toggle auto-firing on/off
    /// </summary>
    public void ToggleFiring()
    {
        if (isFiring)
            StopFiring();
        else
            StartFiring();
    }
    
    // Gizmos for visualization in Scene view
    void OnDrawGizmosSelected()
    {
        if (barrel == null) barrel = transform;
        
        // Draw shooting direction
        Gizmos.color = Color.red;
        Vector3 direction = shootDirection.normalized;
        Gizmos.DrawRay(barrel.position, direction * 2f);
        
        // Draw barrel position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(barrel.position, 0.1f);
    }
}
