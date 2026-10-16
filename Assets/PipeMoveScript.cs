using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed=5f;
    public float deadZone=-20;
    private static float speedBonus;

    public static void ResetSpeedBonus()
    {
        speedBonus = 0f;
    }

    public static void IncreaseSpeed(float amount)
    {
        speedBonus += Mathf.Max(0f, amount);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
    
        transform.position += Vector3.left * (moveSpeed + speedBonus) * Time.deltaTime;
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
