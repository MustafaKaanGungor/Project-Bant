using System;
using UnityEngine;

public class rollPhysic : MonoBehaviour
{
    public static rollPhysic Instance { private set; get; }
    public float WSforce = 5f;
    public float ADforce = 2f;
    public float QEforce = 5f;
    public float maxSpeed = 5f;

    public float jumpForce = 10f;
    private Rigidbody rb;
    Vector3 EulerAngleVelocity;
    public bool isFrozen = false;
    public bool activeGrapple = false;
    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EulerAngleVelocity = new Vector3(100, 0, 0);
        rb.maxAngularVelocity = maxSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFrozen)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        if (activeGrapple)
        {
            return;
        }
        // ! Kaldırılacak, sadece test için burada
        rb.maxAngularVelocity = maxSpeed;
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeTorque(Vector3.right * ADforce);
            //Quaternion deltaRot = Quaternion.Euler(EulerAngleVelocity * Time.deltaTime);
            //rb.MoveRotation(rb.rotation * deltaRot);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.AddRelativeTorque(Vector3.left * ADforce);
            //Quaternion deltaRot = Quaternion.Euler(-EulerAngleVelocity * Time.deltaTime);
            //rb.MoveRotation(rb.rotation * deltaRot);
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

    public void JumpToPosition(Vector3 targetPos, float trajectoryHeight)
    {
        velocityToSet = CalcJumpVelocity(transform.position, targetPos, trajectoryHeight);
        activeGrapple = true;
        Invoke(nameof(SetVelocity), 0.1f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        rb.linearVelocity = velocityToSet;
    }

    public Vector3 CalcJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;

        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) +
        Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
}
