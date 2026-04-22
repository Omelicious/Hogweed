using UnityEngine;

public class GroundRaycast : MonoBehaviour
{

    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private GameObject raycaster;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(raycaster.transform.position, Vector3.down);

        if (Physics.Raycast(ray, out hit, 1.9f, 1 << LayerMask.NameToLayer("Ground"))) // check for ground
        {
            //Debug.Log($"Hit point: {hit.point.y}, distance: {hit.distance}, origin: {ray.origin.y} \n Teleported to the surface");
            
            Vector3 newPosition = transform.position;
            newPosition.y = hit.point.y; // teleporting ourselves to the hitpoint (surface)
            rigidBody.MovePosition(newPosition);
        }
    }
}
