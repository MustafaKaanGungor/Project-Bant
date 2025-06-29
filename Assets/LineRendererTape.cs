using UnityEngine;

public class LineRendererTape : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip;
    public Transform player;
    
    private SpringJoint joint;
    public float maxDistance = 1f;
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void LateUpdate()
    {
        DrawRope();
    }

    private void DrawRope()
    {
        if (lineRenderer.positionCount == 0) return;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, gunTip.position);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartGrapple();

        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            StopGrapple();
        }

        if (lineRenderer.positionCount != 0)
        {
            if (Physics.Raycast(gunTip.position, Vector3.down, out RaycastHit hitInfo, 0.1f, whatIsGrappleable))
            {
                lineRenderer.positionCount += 1;
                lineRenderer.SetPosition(lineRenderer.positionCount - 1, grapplePoint);
                grapplePoint = hitInfo.point;
                lineRenderer.SetPosition(lineRenderer.positionCount - 2, grapplePoint);
                joint.connectedAnchor = grapplePoint;
                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                joint.maxDistance = distanceFromPoint;
                joint.minDistance = distanceFromPoint * 0.25f;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                joint.maxDistance += 0.1f;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                joint.maxDistance -= 0.1f;
            }
        }
    }

    private void StartGrapple()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(player.position, Vector3.down, out raycastHit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = raycastHit.point;
            lineRenderer.positionCount = 2;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.anchor = new Vector3(0, 0, 1);

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;
            lineRenderer.SetPosition(0, gunTip.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    private void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }
    
    /*private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip;

    public Transform player;
    private SpringJoint joint;
    public float maxDistance = 100f;

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

    /*private void StartGrapple()
    {
        {
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
    }*/
}
