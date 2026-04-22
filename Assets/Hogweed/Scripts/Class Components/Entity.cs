using System.Collections;
using Unity.InferenceEngine;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private bool firstHit = true;
    private Rigidbody rigidBody;
    private Collider thisCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        thisCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!firstHit)
            return;

        if (other.CompareTag("Attack Trigger")) // Only if collides with attack
        {
            rigidBody.isKinematic = false;

            rigidBody.AddRelativeTorque(transform.forward * 70f);

            Destroy(gameObject, 3f);

            firstHit = false;
            
            PointsSystem.Instance.PointsTotalIncrease();
        }
    }
}
