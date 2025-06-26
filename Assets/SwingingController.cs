using UnityEngine;

public class SwingingController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip;
    public Transform cam;
    public Transform player;
    private SpringJoint joint;
    public float maxDistance = 100f;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();

        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    private void DrawRope()
    {
        if (!joint) return;
        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }

    private void StartGrapple()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(cam.position, cam.forward, out raycastHit, maxDistance))
        {
            grapplePoint = raycastHit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
        }
    }

    private void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }


}
