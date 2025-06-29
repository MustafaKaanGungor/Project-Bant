using UnityEngine;

public class WheelRoll : MonoBehaviour
{
    [SerializeField] private int accelerationMultiplier = 2;
    public WheelCollider frontLeftWheelCollider;
    private float carSpeed;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float maxReverseSpeed = 10f;
    [SerializeField] private float brakeForce = 5f;
    private float localVelocityX;
    private float localVelocityZ;
    private Rigidbody carRb;
    private bool isDrifting;
    private bool isAccelerating;
    private float throttleAxis;
    private float steeringAxis;
    private float currentSteerAngle = 0;
    [SerializeField] private float steeringSpeed = 5f;
    [SerializeField] private float maxSteeringAngle = 30f;

    private void Awake()
    {
        carRb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }

    void Update()
    {
        float forwardSpeed = Vector3.Dot(transform.forward, carRb.linearVelocity);
        
        carSpeed = 2 * Mathf.PI * frontLeftWheelCollider.radius * frontLeftWheelCollider.rpm * 60 / 1000;
        //Debug.Log(carSpeed);
        localVelocityX = transform.InverseTransformDirection(carRb.linearVelocity).x;

        if (Input.GetKey(KeyCode.W))
        {
            GoForward();
            isAccelerating = Mathf.Sign(+1) == Mathf.Sign(forwardSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            GoReverse();
            isAccelerating = Mathf.Sign(-1) == Mathf.Sign(forwardSpeed);

        }
        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            TurnRight();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Handbrake();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {

        }
        if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            ThrottleOff();
        }
        /*if((!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) && !Input.GetKey(KeyCode.Space) && !deceleratingCar){
            Decelerate();
        }*/
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && steeringAxis != 0f)
        {
            ResetSteering();
        }
    }

    private void GoForward()
    {
        //local velocity x 2.5dan büyükse drift atıyor demek
        if (Mathf.Abs(localVelocityX) > 2.5f)
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        //Gazı bir anda değil yavaşça ver ki pedala basıyormuş gibi olsun
        throttleAxis = throttleAxis + (Time.deltaTime * 3f);
        if (throttleAxis > 1f)
        {
            throttleAxis = 1f;
        }

        //Araç geriye gidiyorsa önce fren bas, sonra gazla
        if (localVelocityZ < -1)
        {
            Debug.Log("frenliyor, ileri");
            Handbrake();
        }
        else
        {
            if (carSpeed < maxSpeed)
            {  //aracın hızı maksdan az ise gaz ver
                frontLeftWheelCollider.motorTorque = accelerationMultiplier * throttleAxis;

                frontLeftWheelCollider.brakeTorque = 0;
            }
            else
            { //Aracın hızı maksimumdan yüksekse daha fazla gaz verme
                frontLeftWheelCollider.motorTorque = 0;

            }

        }
    }

    private void GoReverse()
    {
        //local velocity x 2.5dan büyükse drift atıyor demek
        if (Mathf.Abs(localVelocityX) > 2.5f)
        {
            isDrifting = true;
        }
        else
        {
            isDrifting = false;
        }

        //Gazı bir anda değil yavaşça ver ki pedala basıyormuş gibi olsun
        throttleAxis = throttleAxis - (Time.deltaTime * 3f);
        if (throttleAxis < -1f)
        {
            throttleAxis = -1f;
        }

        //Araç ileri gidiyorsa önce fren bas, sonra gazla
        if (localVelocityZ > 1f)
        {
            Debug.Log("frenliyor, geri");
            Handbrake();
        }
        else
        {
            if (carSpeed > -maxReverseSpeed)
            { //aracın hızı maksdan az ise gaz ver
                frontLeftWheelCollider.motorTorque = accelerationMultiplier * throttleAxis;
                frontLeftWheelCollider.brakeTorque = 0;
            }
            else
            { //Aracın hızı maksimumdan yüksekse daha fazla gaz verme
                frontLeftWheelCollider.motorTorque = 0;
            }

        }
    }

    private void Handbrake()
    {
        frontLeftWheelCollider.brakeTorque = brakeForce;
    }

    private void ThrottleOff()
    {
        frontLeftWheelCollider.motorTorque = 0;
    }

    private void Decelerate()
    {

    }

    private void TurnRight()
    {
        //Direksiyonu yavaşça sağa çevir
        steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
        if (steeringAxis > 1f)
        {
            steeringAxis = 1f;
        }


        //float adjustedspeedFactor = Mathf.InverseLerp(20, maxForwardSpeed, currentSpeed); //minimum speed affecting steerAngle is 20
        //float adjustedTurnAngle = targetSteerAngle * (1 - adjustedspeedFactor); //based on current speed.
        //currentSteerAngle = Mathf.Lerp(currentSteerAngle, adjustedTurnAngle, Time.deltaTime * steeringSpeed);

        //Direksiyon açısını tekerlere uygula

        var steeringAngle = steeringAxis * maxSteeringAngle;
        //float adjustedSpeedFactor = Mathf.InverseLerp();
        //float adjustedTurnAngle = steeringAngle * (1 - adjustedSpeedFactor);
        //currentSteerAngle = Mathf.Lerp(currentSteerAngle, adjustedTurnAngle, Time.deltaTime * steeringSpeed);

        frontLeftWheelCollider.steerAngle = Mathf.Lerp(frontLeftWheelCollider.steerAngle, steeringAngle, steeringSpeed);
    }

    private void TurnLeft()
    {
        //Direksiyonu yavaşça sola çevir
        steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
        if (steeringAxis < -1f)
        {
            steeringAxis = -1f;
        }

        //Direksiyon açısını tekerlere uygula
        var steeringAngle = steeringAxis * maxSteeringAngle;
        frontLeftWheelCollider.steerAngle = Mathf.Lerp(frontLeftWheelCollider.steerAngle, steeringAngle, steeringSpeed);
    }

    private void ResetSteering()
    {
        //Direksiyon açısını zamanla normale döndür
        if (steeringAxis < 0f)
        {
            steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
        }
        else if (steeringAxis > 0f)
        {
            steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
        }
        if (Mathf.Abs(frontLeftWheelCollider.steerAngle) < 1f)
        {
            steeringAxis = 0f;
        }
        
        //Direksiyon açısını tekerlere uygula
        var steeringAngle = steeringAxis * maxSteeringAngle;
        frontLeftWheelCollider.steerAngle = Mathf.Lerp(frontLeftWheelCollider.steerAngle, steeringAngle, steeringSpeed);
    }
}
