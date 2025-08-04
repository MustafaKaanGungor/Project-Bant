using CMF;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public SimpleWalkerController playerController;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;

    public float maxGrappleDistance;
    public float grappleDelayTime;

    private Vector3 grapplePoint;

    public float grapplingCd;
    private float grapplingCdTimer;

    public KeyCode grappleInput = KeyCode.Mouse1;

    private bool isGrappling;

    public LineRenderer lineRenderer;
    public float overshootYAxis;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(grappleInput))
        {
            StartGrapple();
        }

        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0)
        {
            return;
        }

        isGrappling = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(1, grapplePoint);
        //playerController.isFrozen = true;
    }

    void LateUpdate()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, gunTip.position);
        }
    }

    private void ExecuteGrapple()
    {
        //playerController.isFrozen = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativePos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativePos + overshootYAxis;

        if (grapplePointRelativePos < 0)
        {
            highestPointOnArc = overshootYAxis;
        }

        playerController.JumpToPosition(grapplePoint, highestPointOnArc);
        Invoke(nameof(StopGrapple), 1f);
    }

    private void StopGrapple()
    {
        isGrappling = false;
        //playerController.isFrozen = false;
        //playerController.activeGrapple = false;

        grapplingCdTimer = grapplingCd;
        lineRenderer.enabled = false;        
    }
}
