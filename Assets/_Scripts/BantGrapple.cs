using UnityEngine;

public class BantGrapple : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Rigidbody playerRb;
    private LineRenderer lineRenderer;
    [SerializeField] private Camera mainCam;
    [SerializeField] private Transform firePoint;
    private Vector3 grapplePoint;
    private SpringJoint joint;
    [SerializeField] private LayerMask whatIsGrappleable;
    [SerializeField] private float maxGrappleDistance = 100;
    [SerializeField] private KeyCode swingInput = KeyCode.Mouse0;
    [SerializeField] private KeyCode grappleInput = KeyCode.Mouse1;
    private bool isGrappling = false;
    private bool isSwinging = false;
    [SerializeField] private float verticalOvershoot;
    private float grappleCdTimer = 0;
    [SerializeField] private float grappleMaxCD = 1;

    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(swingInput))
        {
            isSwinging = true;
            StartSwinging();
        }
        if (Input.GetKeyUp(swingInput))
        {
            isSwinging = false;
            StopSwinging();
        }

        if (Input.GetKeyDown(grappleInput) && isSwinging && grappleCdTimer <= 0)
        {
            isGrappling = true;
            StartGrapple();
        }

        if (grappleCdTimer > 0)
        {
            grappleCdTimer -= Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        if (isSwinging)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    private void StartSwinging()
    {
        RaycastHit hit;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            joint = player.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
        }
    }

    private void StopSwinging()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    private void StartGrapple()
    {
        // oyuncu dondur
        ExecuteGrapple();
    }

    private void ExecuteGrapple()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativePos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativePos + verticalOvershoot;

        if (grapplePointRelativePos < 0)
        {
            highestPointOnArc = verticalOvershoot;
        }

        //jump position at
        Invoke(nameof(StopGrapple), 1f);
    }

    private void StopGrapple()
    {
        //oyuncuyu geri dondurmaktan çıkar
        isGrappling = false;
        lineRenderer.enabled = false;
    }
}
