using UnityEngine;

public class StickTapeController : MonoBehaviour
{
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip;
    public Transform cam;
    public Transform player;
    public float maxDistance = 1f;
    public GameObject tapeHeadPrefab;
    private GameObject tapeHead;
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && tapeHead == null)
        {
            StartGrapple();

        }
        else if (Input.GetKeyDown(KeyCode.F) && tapeHead != null)
        {
            StopGrapple();
        }
    }

    private void StartGrapple()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(player.position, Vector3.down, out raycastHit, maxDistance))
        {
            grapplePoint = raycastHit.point;
            tapeHead = Instantiate(tapeHeadPrefab, grapplePoint, Quaternion.identity);
            tapeHead.GetComponent<HingeJoint>().connectedBody = player.GetComponent<Rigidbody>();
        }
    }

    private void StopGrapple()
    {
        Destroy(tapeHead);
        tapeHead = null;    
    }
}
