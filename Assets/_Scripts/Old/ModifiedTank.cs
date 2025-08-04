using UnityEngine;

public class ModifiedTank : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f; 
    private float m_MovementInputValue;
    private float m_TurnInputValue;  
    
    private void Awake() {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
        Turn();

        
    }

    private void GetInput()
    {
        m_MovementInputValue = Input.GetAxis("Vertical");
        m_TurnInputValue = Input.GetAxis("Horizontal");
    }
    
    private void Move()
    {
        float speedInput = 0.0f;
        speedInput = m_MovementInputValue;

        Vector3 movement = transform.forward * speedInput * m_Speed * Time.deltaTime;

        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn ()
    {
        Quaternion turnRotation;
            
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        turnRotation = Quaternion.Euler (0f, turn, 0f);
            

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }
}
