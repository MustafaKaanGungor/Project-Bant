using UnityEngine;

public class floatingMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]private Vector3 downDir;

    public float RideHeight;
    public float RideSpringStrength;
    public float RideSpringDamper;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        test();
    }

    void test()
    {
        var _rayDidHit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit rayHit);
        if (_rayDidHit)
        {
            Vector3 vel = rb.linearVelocity;
            Vector3 rayDir = transform.TransformDirection(downDir);

            Vector3 otherVel = Vector3.zero;
            Rigidbody hitbody = rayHit.rigidbody;
            if (hitbody != null)
            {
                otherVel = hitbody.linearVelocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relvel = rayDirVel - otherDirVel;

            float x = rayHit.distance - RideHeight;

            float springForce = (x * RideSpringStrength) - (relvel * RideSpringDamper);

            Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.cyan);

            rb.AddForce(rayDir * springForce);

            if (hitbody != null)
            {
                hitbody.AddForceAtPosition(rayDir * -springForce, rayHit.point);
            }


        }
    }
}
