using UnityEngine;
using System.Collections.Generic;
public class PlatformMovement : MonoBehaviour
{
	[SerializeField] private float movementSpeed = 10f;
	[SerializeField] private bool reverseDirection = false;
	private Rigidbody rb;
	[SerializeField] private List<Transform> waypoints = new List<Transform>();
	private int currentWaypointIndex = 0;
	private Transform currentWaypoint;
	[SerializeField] private PlatformTrigger trigger;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		rb.useGravity = false;
		rb.isKinematic = true;

		currentWaypoint = waypoints[currentWaypointIndex];
	}

	private void Start()
	{

	}

    void FixedUpdate()
    {
		MovePlatform();
    }

	private void MovePlatform()
	{
		if (waypoints.Count <= 0)
		{
			return;
		}

		Vector3 towardWaypoint = currentWaypoint.position - transform.position;
		Vector3 movementVector = towardWaypoint.normalized;

		movementVector *= movementSpeed * Time.deltaTime;

		if (movementVector.magnitude >= towardWaypoint.magnitude || movementVector.magnitude == 0f)
		{
			rb.transform.position = currentWaypoint.position;
			UpdateWaypoint();
		}
		else
		{
			rb.transform.position += movementVector;
		}

		if (trigger.detectedRigidbody != null)
		{
			trigger.detectedRigidbody.MovePosition(trigger.detectedRigidbody.position + movementVector);
		}
	}

	private void UpdateWaypoint()
	{
		if (reverseDirection)
		{
			currentWaypointIndex--;
		}
		else
		{
			currentWaypointIndex++;
		}

		if (currentWaypointIndex >= waypoints.Count)
		{
			currentWaypointIndex = 0;
		}
		if (currentWaypointIndex < 0)
		{
			currentWaypointIndex = waypoints.Count - 1;
		}

		currentWaypoint = waypoints[currentWaypointIndex];
	}
}
