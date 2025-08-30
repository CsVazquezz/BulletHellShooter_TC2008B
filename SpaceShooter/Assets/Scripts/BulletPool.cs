using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet pool system for efficient bullet management.
/// Creates a pool of bullets at start and reuses them instead of instantiating/destroying.
/// </summary>
public class BulletPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialPoolSize = 200; // Increased for bullet hell
    [SerializeField] private bool canExpand = true;
    [SerializeField] private int maxPoolSize = 500; // Increased for bullet hell
    
    private List<GameObject> allBullets = new List<GameObject>();
    
    [Header("Debug Info")]
    [SerializeField] private int activeBullets = 0;
    [SerializeField] private int totalBullets = 0;
    
    void Awake()
    {
        InitializePool();
    }
    
    /// <summary>
    /// Create initial pool of bullets
    /// </summary>
    private void InitializePool()
    {
        // Create parent object to organize bullets in hierarchy
        GameObject bulletContainer = new GameObject("Bullet Container");
        bulletContainer.transform.SetParent(transform);
        
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewBullet(bulletContainer.transform);
        }
        
        totalBullets = initialPoolSize;
        Debug.Log($"Bullet pool initialized with {initialPoolSize} bullets");
    }
    
    /// <summary>
    /// Create a new bullet and add it to the pool
    /// </summary>
    private GameObject CreateNewBullet(Transform parent = null)
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned in BulletPool!");
            return null;
        }
        
        GameObject bullet = Instantiate(bulletPrefab, parent ? parent : transform);
        bullet.SetActive(false);
        
        // Ensure bullet has required components
        if (bullet.GetComponent<Bullet>() == null)
        {
            Debug.LogWarning("Bullet prefab doesn't have a Bullet component. Adding one.");
            bullet.AddComponent<Bullet>();
        }
        
        // Ensure collider is set as trigger
        Collider2D col = bullet.GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        
        allBullets.Add(bullet);
        
        return bullet;
    }
    
    /// <summary>
    /// Get an available bullet from the pool
    /// </summary>
    /// <returns>Active bullet ready to use</returns>
    public Bullet RequestBullet()
    {
        // Try to find an inactive bullet
        for (int i = 0; i < allBullets.Count; i++)
        {
            if (!allBullets[i].activeInHierarchy)
            {
                allBullets[i].SetActive(true);
                activeBullets++;
                return allBullets[i].GetComponent<Bullet>();
            }
        }
        
        // If no inactive bullets available and can expand, create new one
        if (canExpand && allBullets.Count < maxPoolSize)
        {
            GameObject bullet = CreateNewBullet();
            if (bullet != null)
            {
                bullet.SetActive(true);
                activeBullets++;
                totalBullets++;
                Debug.Log($"Pool expanded. Total bullets: {totalBullets}");
                return bullet.GetComponent<Bullet>();
            }
        }
        
        Debug.LogWarning("Could not get bullet from pool!");
        return null;
    }
    
    /// <summary>
    /// Return a bullet to the pool (simplified)
    /// </summary>
    /// <param name="bullet">Bullet to return</param>
    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null) return;
        
        bullet.SetActive(false);
        activeBullets = Mathf.Max(0, activeBullets - 1);
    }
    
    /// <summary>
    /// Get bullet and initialize it with direction - simplified version
    /// </summary>
    /// <param name="position">Starting position</param>
    /// <param name="direction">Movement direction</param>
    /// <param name="speed">Bullet speed</param>
    /// <returns>Initialized bullet</returns>
    public Bullet ShootBullet(Vector3 position, Vector2 direction, float speed = 10f)
    {
        Bullet bullet = RequestBullet();
        if (bullet != null)
        {
            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.identity;
            
            bullet.velocity = direction.normalized * speed;
            bullet.SetBulletPool(this);
            
            Debug.Log($"Bullet shot from {position} with velocity {bullet.velocity}");
        }
        
        return bullet;
    }
    
    /// <summary>
    /// Get current pool statistics
    /// </summary>
    public void GetPoolStats(out int active, out int available, out int total)
    {
        active = activeBullets;
        available = totalBullets - activeBullets; // Calculate available from total minus active
        total = totalBullets;
    }
    
    /// <summary>
    /// Debug method to show pool status in inspector
    /// </summary>
    void Update()
    {
        if (Application.isPlaying)
        {
            // Count active bullets manually
            int activeCount = 0;
            for (int i = 0; i < allBullets.Count; i++)
            {
                if (allBullets[i].activeInHierarchy)
                    activeCount++;
            }
            activeBullets = activeCount;
        }
    }
}
