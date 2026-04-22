using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int hp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Attack Trigger"))
        {
            hp--;
            if (hp <= 0)
            {
                // Do our own stuff?
                HealthSystem.Instance.Death(gameObject);
            }
        }
    }
}
