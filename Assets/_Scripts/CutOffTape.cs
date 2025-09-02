using System.Collections.Generic;
using UnityEngine;

public class CutOffTape : MonoBehaviour
{
    private Rigidbody rb;
    private LineRenderer lineRenderer;
    private Collider itemCollider;
    private SpringJoint joint;
    private float distanceFromPoint;
    private Vector3 holdPoint;
    private LayerMask whatIsGround;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        itemCollider = GetComponent<Collider>();
    }

    private void Update() {
        if (Physics.Raycast(holdPoint, transform.position - holdPoint, out RaycastHit raycastHit, 100f))
        {
            if (raycastHit.collider != itemCollider.GetComponent<Collider>())
            {
                Physics.Raycast(transform.position, holdPoint - transform.position, out RaycastHit newHit, 100f, whatIsGround);
                    
                //AddNewStickPoint(newHit.point);
                //SetupJoint(newHit.point);
            }
        }
    }

    void LateUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);
    }

    public void SetTapePart(Vector3[] positions)
    {
        int count = positions.Length;
        lineRenderer.positionCount = count;
        lineRenderer.SetPositions(positions);


        holdPoint = positions[positions.Length - 1];

        joint = gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = holdPoint;

        distanceFromPoint = Vector3.Distance(transform.position, holdPoint);

        joint.maxDistance = distanceFromPoint * 0.3f;
        joint.minDistance = distanceFromPoint * 0.2f;

        // !joint.spring = PlayerMovement.Instance.spring;
        // !joint.damper = PlayerMovement.Instance.damper;
        // !joint.massScale = PlayerMovement.Instance.massScale;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, holdPoint);
    }
}
