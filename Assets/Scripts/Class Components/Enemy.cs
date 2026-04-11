using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Rigidbody targetRigidBody;
    private Rigidbody rigidBody;
    [SerializeField] private Transform raycaster;
    private Vector3 moveVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        target = GameObject.FindWithTag("Player");
        targetRigidBody = target.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        GoToTarget();
    }

    void GoToTarget()
    {
        // Calculate vector
        moveVector = 5f * Time.deltaTime * Vector3.ClampMagnitude(target.transform.position - gameObject.transform.position, 1f);
        // Move Rigidbody
        rigidBody.MovePosition(gameObject.transform.position + moveVector);

        FixGroundCollision();
    }

    void FixGroundCollision () // Raycasting ground. If player is sinking, moving him up to the surface
    {
        RaycastHit hit;
        Ray ray = new Ray(raycaster.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit, 0.9f, 1 << LayerMask.NameToLayer("Ground"))) // check for ground
        {
            //Debug.Log($"Hit point: {hit.point.y}, distance: {hit.distance}, origin: {ray.origin.y} \n Teleported to the surface");
            
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y + 0.5f; // teleporting ourselves to the hitpoint (surface)
            rigidBody.MovePosition(newPosition);
        }
    }
}
