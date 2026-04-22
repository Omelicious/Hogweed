using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death(GameObject damagable)
    {
        Destroy(damagable);
    }
}
