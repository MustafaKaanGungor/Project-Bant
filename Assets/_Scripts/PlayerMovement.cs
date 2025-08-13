using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody rb;

    [Header("Children")]
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject playerCollider;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7;
    [SerializeField] private float groundDrag = 2;
    [SerializeField] private float turnSpeed = 2;
    private Vector2 movementInput = Vector2.zero;
    private Vector3 moveDirection = Vector3.zero;

    [Header("Visuals")]
    [SerializeField] private float modelRollModifier = 1f;
    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 4;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded = false;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f;
    private bool readyToJump = true;

    [Header("Slope")]
    [SerializeField] private float maxSlopeAngle = 40f;
    private RaycastHit slopeHit;
    private bool exitingSlope = false;

    [Header("Rebalance")]
    [SerializeField] private float reflection = 100f;
    [SerializeField] private float stability = 0.5f;
    [SerializeField] private float rebalanceLimit = 0.3f;

    [Header("Looking and Aiming")]
    [SerializeField] private GameObject targetPoint;
    [SerializeField] private GameObject freeLookCam;
    [SerializeField] private GameObject thirdPersonCam;
    private bool isAiming = false;
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    [SerializeField] private float dampingMultiplier;
    private float yRotation;
    private float xRotation;
    private Vector3 thirdPersonLastDirection;

    [Header("Swing and Grapple")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float maxGrappleDistance = 100;
    private bool isSwinging = false;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    [SerializeField] private float minDistanceMultiplier = 0.25f;
    [SerializeField] private float maxDistanceMultiplier = 0.8f;
    [SerializeField] private float spring = 4.5f;
    [SerializeField] private float damper = 7f;
    [SerializeField] private float massScale = 4.5f;
    private float distanceFromPoint = 0;
    [SerializeField] private LayerMask cutterLayer;
    private float tapeAmount = 0;
    [SerializeField] private float tapeSpentMultiplier = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameInput.Instance.OnJumpPerformed += on_jump_performed;
        GameInput.Instance.OnAimPerformed += on_aim_performed;
        GameInput.Instance.OnAimCanceled += on_aim_canceled;
        GameInput.Instance.OnFirePerformed += on_fire_performed;
        GameInput.Instance.OnFireCanceled += on_fire_canceled;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Aiming();
        GroundCheck();
        GetMovementVector();
        SpeedControl();
        CutOff();
        TapeVisual();
    }

    private void TapeVisual()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        float forwardSpeed = localVelocity.z;
        float rotationAmount = forwardSpeed * modelRollModifier;
        playerModel.transform.Rotate(0f, -rotationAmount, 0f, Space.Self);
    }

    private void CutOff()
    {
        if (isSwinging)
        {
            Debug.DrawRay(firePoint.position, (grapplePoint - firePoint.position).normalized);
            if (Physics.Raycast(firePoint.position, (grapplePoint - firePoint.position).normalized, out RaycastHit hitInfo, distanceFromPoint, cutterLayer))
            {
                Debug.Log(hitInfo.collider);
                StopSwinging();
            }
        }
    }

    private void Aiming()
    {
        if (isAiming)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            yRotation = transform.eulerAngles.y + mouseX;
            transform.rotation = Quaternion.Euler(0, yRotation, 0);

            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            targetPoint.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        }

    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, -transform.up, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Rebalance();
    }

    private void LateUpdate()
    {
        if (isSwinging)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    private void on_jump_performed(object sender, EventArgs e)
    {
        if (readyToJump && grounded)
        {
            exitingSlope = true;
            readyToJump = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private void GetMovementVector()
    {
        movementInput = GameInput.Instance.GetMovementVector();
    }

    private void MovePlayer()
    {
        Vector3 newForward = new Vector3(transform.forward.x, 0, transform.forward.z);
        moveDirection = newForward.normalized * movementInput.y;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        transform.Rotate(new Vector3(0, movementInput.x * turnSpeed, 0));
        rb.useGravity = !OnSlope();
    }


    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            //TODO max speed dene
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, -transform.up, out slopeHit, playerHeight * 0.5f + 0.2f, whatIsGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <= maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void Rebalance()
    {
        Vector3 predictedUp = Quaternion.AngleAxis(rb.linearVelocity.magnitude * Mathf.Rad2Deg * stability / reflection, rb.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        if (!grounded && torqueVector.magnitude >= rebalanceLimit)
        {
            rb.AddTorque(torqueVector * reflection * (torqueVector.magnitude - rebalanceLimit));
        }
    }

    private void on_fire_canceled(object sender, EventArgs e)
    {
        StopSwinging();
    }

    private void on_fire_performed(object sender, EventArgs e)
    {
        isSwinging = true;
        StartSwinging();
    }

    private void on_aim_performed(object sender, EventArgs e)
    {
        isAiming = true;
        thirdPersonLastDirection = transform.position - mainCam.transform.position;
        thirdPersonLastDirection = new Vector3(thirdPersonLastDirection.x, 0, thirdPersonLastDirection.z);
        transform.LookAt(transform.position + thirdPersonLastDirection);
        thirdPersonCam.GetComponent<CinemachineCamera>().Prioritize();
    }

    private void on_aim_canceled(object sender, EventArgs e)
    {
        isAiming = false;
        freeLookCam.GetComponent<CinemachineCamera>().Prioritize();

    }

    private void StartSwinging()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, maxGrappleDistance, whatIsGround))
        {
            grapplePoint = hit.point;

            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;

            tapeAmount += distanceFromPoint / 10 * tapeSpentMultiplier;
            tapeAmount = Mathf.Clamp(tapeAmount, 0, 100);
            playerModel.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(2, tapeAmount);
            playerCollider.transform.localScale = new Vector3(10 - tapeAmount / 10 + 21, 30, 10 - tapeAmount / 10 + 21);
        }
    }

    private void StopSwinging()
    {
        isSwinging = false;
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }
}
