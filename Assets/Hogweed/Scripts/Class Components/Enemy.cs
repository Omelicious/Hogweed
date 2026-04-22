using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 8f;

    [SerializeField] private GameObject attackEffect;
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
        targetRigidBody = target?.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (target != null)
            GoToTarget();
    }

    void GoToTarget()
    {
        // Calculate vector
        moveVector.x = target.transform.position.x - gameObject.transform.position.x;
        moveVector.y = 0f;
        moveVector.z = target.transform.position.z - gameObject.transform.position.z;

        if (moveVector.magnitude < 4.5f) // stop if close
        {
            Attack(moveVector);
            return;
        }
        
        moveVector = speed * Time.deltaTime * Vector3.ClampMagnitude(moveVector, 1f);
        // Move Rigidbody
        rigidBody.MoveRotation(Quaternion.Euler(0f, Vector3.Angle(rigidBody.rotation.eulerAngles, moveVector), 0f));
        rigidBody.MovePosition(gameObject.transform.position + moveVector);

        FixGroundCollision();
    }

    void Attack(Vector3 direction)
    {
        direction.y = -1;
        if (gameObject.transform.Find("Attack Effect(Clone)") == false) // If there's no attacks present
        {
            direction.y = -3f;
            GameObject attackInstance = Instantiate(attackEffect, gameObject.transform.position + direction, rigidBody.rotation, gameObject.transform);
            
            Destroy(attackInstance, 1.5f);
        }
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
