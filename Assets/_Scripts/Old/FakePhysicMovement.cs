using UnityEngine;
using UnityEngine.InputSystem;

public class FakePhysicMovement : MonoBehaviour
{
    private Rigidbody rb;
    Vector3 inputVector = Vector3.zero;
    bool isSpacePressed = false;

    private void Awake()
    {
        rb.GetComponent<Rigidbody>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        GetInputs();
    }

    private void GetInputs()
    {
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");


        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
        {
            inputVector.z = 0;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            inputVector.z = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            inputVector.z = 1;
        }
        else
        {
            inputVector.z = 0;
        }
    }
    
    private void FixedUpdate()
    {
        
    }
}
