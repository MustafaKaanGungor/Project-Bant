using System;
using UnityEngine;

public class TapeMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float groundDrag;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded = false;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump = true;
    [SerializeField] private Transform orientation;
    private Vector2 movementInput = Vector2.zero;
    private Vector3 moveDirection;
    private Rigidbody rb;
    [SerializeField]private float landingGravityMultiplier;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        GameInput.Instance.OnJumpPerformed += on_jump_performed;
    }

    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        LimitSpeed();

        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0f;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }
    
    private void on_jump_performed(object sender, EventArgs e)
    {
        if (readyToJump && isGrounded)
        {
            readyToJump = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MyInput()
    {
        movementInput = GameInput.Instance.GetMovementVector();
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * movementInput.y;

        if (isGrounded)
        {
            rb.AddForce(10f * moveSpeed * moveDirection.normalized, ForceMode.Force);
        }
        else
        {
            rb.AddForce(10f * airMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Force);
            //rb.AddForce(Vector3.down * landingGravityMultiplier, ForceMode.Force);

        }
        
        transform.Rotate(new Vector3(0, movementInput.x * 2, 0));
    }
    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, 0, limitedVel.z);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
