using System;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.Tools;
using Unity.Cinemachine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; private set; }
    [Header("Stats")]
    [SerializeField] private PlayerStatsSO playerStats;

    [Header("Components")]
    private Rigidbody rb;

    [Header("Children")]
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject playerRunOutModel;
    [SerializeField] private GameObject playerCollider;

    [Header("Movement")]
    private Vector2 movementInput = Vector2.zero;
    private Vector3 moveDirection = Vector3.zero;

    [Header("Visuals")]
    [SerializeField] private GameObject speedLinesEffect;

    [Header("Ground Check")]
    private bool grounded = false;

    [Header("Jump")]
    private bool readyToJump = true;

    [Header("Slope")]
    private RaycastHit slopeHit;
    private bool exitingSlope = false;

    [Header("Looking and Aiming")]
    [SerializeField] private GameObject targetPoint;
    [SerializeField] private GameObject freeLookCam;
    [SerializeField] private GameObject thirdPersonCam;
    private bool isAiming = true;
    [SerializeField] private float dampingMultiplier;
    private float yRotation;
    private float xRotation;
    private Vector3 thirdPersonLastDirection;

    [Header("Swing and Grapple")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform firePoint;
    private bool isSwinging = false;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    private float distanceFromPoint = 0;
    private float tapeAmount = 0;
    [SerializeField] private GameObject cuttOffTape;
    [SerializeField] private MMSquashAndStretch mMSquash;
    public event EventHandler OnTapeAmountChange;
    [SerializeField] private GameObject tapedImage;
    public event EventHandler OnGameEnd;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameInput.Instance.OnJumpPerformed += on_jump_performed;
        GameInput.Instance.OnAimPerformed += on_aim_performed;
        GameInput.Instance.OnAimCanceled += on_aim_canceled;
        GameInput.Instance.OnFirePerformed += on_fire_performed;
        GameInput.Instance.OnFireCanceled += on_fire_canceled;

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
        if (GameManager.Instance.IsPlaying())
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
            float forwardSpeed = localVelocity.z;
            float rotationAmount = forwardSpeed * playerStats.modelRollModifier;
            playerModel.transform.Rotate(0f, -rotationAmount, 0f, Space.Self);

            if (rb.linearVelocity.magnitude > playerStats.moveSpeed * playerStats.effectLimitMultiplier)
            {
                speedLinesEffect.SetActive(true);
            }
            else
            {
                speedLinesEffect.SetActive(false);
            }
        }
    }

    private void CutOff()
    {
        if (isSwinging)
        {
            Debug.DrawRay(firePoint.position, (grapplePoint - firePoint.position).normalized);
            if (Physics.Raycast(firePoint.position, (grapplePoint - firePoint.position).normalized, out RaycastHit hitInfo, distanceFromPoint, playerStats.cutterLayer))
            {
                Debug.Log(hitInfo.collider);
                StopSwinging();
            }

            if (Physics.Raycast(grapplePoint, transform.position - grapplePoint, out RaycastHit raycastHit, 100f))
            {
                if (raycastHit.collider != playerCollider.GetComponent<Collider>())
                {
                    Physics.Raycast(transform.position, grapplePoint - transform.position, out RaycastHit newHit, 100f, playerStats.whatIsGround);

                    AddNewStickPoint(newHit.point);
                    SetupJoint(newHit.point);
                }
                else
                {
                    //Debug.Log(raycastHit.collider);
                    //Debug.Log("heyo");
                }
            }
        }
    }

    private void Aiming()
    {
        if (isAiming)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * playerStats.sensX;
            yRotation = transform.eulerAngles.y + mouseX;
            //transform.rotation = Quaternion.Euler(0, yRotation, 0);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);

            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * playerStats.sensY;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            targetPoint.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        }

    }

    private void GroundCheck()
    {

        grounded = Physics.Raycast(transform.position, -transform.up, playerStats.playerHeight * 0.5f + 0.2f, playerStats.whatIsGround);



        if (grounded)
        {
            rb.linearDamping = playerStats.groundDrag;
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
            //lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, firePoint.position);

        }
    }

    private void on_jump_performed(object sender, EventArgs e)
    {
        if (readyToJump && grounded)
        {
            exitingSlope = true;
            readyToJump = false;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            rb.AddForce(transform.up * playerStats.jumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), playerStats.jumpCooldown);
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
            rb.AddForce(GetSlopeMoveDirection() * playerStats.moveSpeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * playerStats.moveSpeed * 10, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * playerStats.moveSpeed * 10f * playerStats.airMultiplier, ForceMode.Force);
        }
;
        rb.useGravity = !OnSlope();
    }


    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.linearVelocity.magnitude > playerStats.moveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * playerStats.moveSpeed;
            }
        }
        else if (grounded)
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            //TODO max speed dene
            if (flatVel.magnitude > playerStats.moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * playerStats.moveSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            //TODO max speed dene
            if (flatVel.magnitude > playerStats.moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * playerStats.moveSpeed * playerStats.airMaxSpeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, -transform.up, out slopeHit, playerStats.playerHeight * 0.5f + 0.2f, playerStats.whatIsGround))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle <= playerStats.maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void Rebalance()
    {
        Vector3 predictedUp = Quaternion.AngleAxis(rb.linearVelocity.magnitude * Mathf.Rad2Deg * playerStats.stability / playerStats.reflection, rb.angularVelocity) * transform.up;
        Vector3 torqueVector = Vector3.Cross(predictedUp, Vector3.up);
        if (!grounded && torqueVector.magnitude >= playerStats.rebalanceLimit)
        {
            rb.AddTorque(torqueVector * playerStats.reflection * (torqueVector.magnitude - playerStats.rebalanceLimit));
        }
    }

    private void on_fire_canceled(object sender, EventArgs e)
    {
        if (isSwinging)
        {
            StopSwinging();
        }

    }

    private void on_fire_performed(object sender, EventArgs e)
    {
        if (IsLookingAtGrappleable())
        {
            isSwinging = true;
            StartSwinging();
        }

    }

    private void on_aim_performed(object sender, EventArgs e)
    {
        isAiming = false;
        freeLookCam.GetComponent<CinemachineCamera>().Prioritize();
    }

    private void on_aim_canceled(object sender, EventArgs e)
    {
        isAiming = true;
        thirdPersonLastDirection = transform.position - mainCam.transform.position;
        thirdPersonLastDirection = new Vector3(thirdPersonLastDirection.x, 0, thirdPersonLastDirection.z);
        transform.LookAt(transform.position + thirdPersonLastDirection);
        thirdPersonCam.GetComponent<CinemachineCamera>().Prioritize();
    }

    private void StartSwinging()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, playerStats.maxGrappleDistance, playerStats.whatIsGround))
        {
            if (hit.collider.CompareTag("Cup"))
            {
                hit.collider.GetComponent<Cup>().GetTapedIdiot();
            }
            grapplePoint = hit.point;

            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * playerStats.maxDistanceMultiplier;
            joint.minDistance = distanceFromPoint * playerStats.minDistanceMultiplier;

            joint.spring = playerStats.spring;
            joint.damper = playerStats.damper;
            joint.massScale = playerStats.massScale;

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, grapplePoint);

            Instantiate(tapedImage, grapplePoint, Quaternion.Euler(new Vector3(hit.normal.x, hit.normal.y + 90, hit.normal.z + 90)));

            tapeAmount += distanceFromPoint / 10 * playerStats.tapeSpentMultiplier;
            tapeAmount = Mathf.Clamp(tapeAmount, 0, 100);
            OnTapeAmountChange?.Invoke(this, EventArgs.Empty);
            playerModel.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(2, tapeAmount);
            playerCollider.transform.localScale = new Vector3(10 - tapeAmount / 10 + 21, 30, 10 - tapeAmount / 10 + 21);

            if (tapeAmount >= 100)
            {
                playerModel.SetActive(false);
                playerRunOutModel.SetActive(true);
                OnGameEnd?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void SetupJoint(Vector3 hitpoint)
    {
        grapplePoint = hitpoint;

        joint.connectedAnchor = grapplePoint;

        distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;
    }

    private void AddNewStickPoint(Vector3 point)
    {
        lineRenderer.positionCount++;

        lineRenderer.SetPosition(lineRenderer.positionCount - 1, grapplePoint);
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, point);
    }

    private void StopSwinging()
    {
        isSwinging = false;
        GameObject tapePart = Instantiate(cuttOffTape, targetPoint.transform.position, Quaternion.identity);
        Vector3[] positionArray = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positionArray);
        List<Vector3> positionList = positionArray.ToList();
        tapePart.GetComponent<CutOffTape>().SetTapePart(positionArray);
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    public bool IsLookingAtGrappleable()
    {
        return Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, playerStats.maxGrappleDistance, playerStats.whatIsGround);
    }

    public float HowMuchTapeLeft()
    {
        return 100 - tapeAmount;
    }
}
