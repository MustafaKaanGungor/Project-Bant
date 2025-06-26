using UnityEngine;

public class rollPhysic : MonoBehaviour
{
    public float WSforce = 5f;
    public float ADforce = 2f;
    public float QEforce = 5f;

    public float jumpForce = 10f;
    private Rigidbody rb;
    Vector3 EulerAngleVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EulerAngleVelocity = new Vector3(100, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            //rb.AddRelativeTorque(Vector3.right * ADforce);
            Quaternion deltaRot = Quaternion.Euler(EulerAngleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRot);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //rb.AddRelativeTorque(Vector3.left * ADforce);
            Quaternion deltaRot = Quaternion.Euler(-EulerAngleVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRot);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeTorque(Vector3.down * WSforce);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeTorque(Vector3.up * WSforce);
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            rb.AddRelativeTorque(Vector3.forward * QEforce);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rb.AddRelativeTorque(Vector3.back * QEforce);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
    }
}
