using UnityEngine;

public class TapeVisual : MonoBehaviour
{
    public SCC_Drivetrain drivetrain;
    public WheelCollider wheelCollider;
    private float speed;
    public float multiplier = 0.5f;
    private float rotSpeed = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Start()
    {
        rotSpeed = wheelCollider.rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        rotSpeed = wheelCollider.rotationSpeed;
        
        transform.Rotate(new Vector3(rotSpeed * multiplier, 0, 0));
        //var deltaRotSpeed = rotSpeed - wheelCollider.rotationSpeed / Time.deltaTime;
        //rotSpeed = wheelCollider.rotationSpeed / Time.deltaTime;

        //transform.Rotate(deltaRotSpeed, 0, 0);
        //wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        //transform.SetPositionAndRotation(transform.position, rot);
    }

    
}
