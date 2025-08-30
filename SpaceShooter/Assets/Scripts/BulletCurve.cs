using UnityEngine;

/// <summary>
/// Optional component that adds curve behavior to bullets.
/// Makes bullets follow curved paths for more interesting patterns.
/// </summary>
public class BulletCurve : MonoBehaviour
{
    [Header("Curve Settings")]
    public float curveStrength = 0f;
    
    private Bullet bullet;
    
    void Awake()
    {
        bullet = GetComponent<Bullet>();
    }
    
    void Update()
    {
        if (bullet == null || Mathf.Abs(curveStrength) < 0.0001f) return;
        
        // Apply curve effect by adjusting velocity perpendicular to current direction
        if (bullet.velocity.sqrMagnitude > 0.000001f)
        {
            Vector3 perpendicular = new Vector3(-bullet.velocity.y, bullet.velocity.x, 0f).normalized;
            bullet.velocity -= perpendicular * (curveStrength * Time.deltaTime);
        }
    }
}
