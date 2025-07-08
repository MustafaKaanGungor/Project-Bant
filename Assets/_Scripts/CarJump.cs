using UnityEngine;

public class CarJump : MonoBehaviour
{
    private SCC_Wheel[] wheels;
    private Rigidbody rb;
    public float jumpForce = 10f;
    private void Start()
    {

        wheels = GetComponentsInChildren<SCC_Wheel>();
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        bool grounded = false;

        for (int i = 0; i < wheels.Length; i++)
        {

            if (wheels[i].isGrounded)
                grounded = true;

        }

        if (grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce);
            }
        }
    }
}
