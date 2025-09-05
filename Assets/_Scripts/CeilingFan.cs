using UnityEngine;

public class CeilingFan : MonoBehaviour
{
    [SerializeField] private float turnSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(0 ,1 ,0) * turnSpeed * Time.deltaTime);
    }
}
