using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "PlayerStatsSO", order = 0)]
public class PlayerStatsSO : ScriptableObject
{
     [Header("Movement")]
    public float moveSpeed = 7;
    public float groundDrag = 2;
    public float turnSpeed = 2;
    public float airMaxSpeed;

    [Header("Visuals")]
    public float modelRollModifier = 1f;
    public float effectLimitMultiplier = 1f;
    [Header("Ground Check")]
    public float playerHeight = 4;
    public LayerMask whatIsGround;

    [Header("Jump")]
    public float jumpForce = 12;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;

    [Header("Slope")]
    public float maxSlopeAngle = 40f;

    [Header("Rebalance")]
    public float reflection = 100f;
    public float stability = 0.5f;
    public float rebalanceLimit = 0.3f;

    [Header("Looking and Aiming")]
    public float sensX;
    public float sensY;
    public float dampingMultiplier;

    [Header("Swing and Grapple")]
    public float maxGrappleDistance = 100;
    public float minDistanceMultiplier = 0.25f;
    public float maxDistanceMultiplier = 0.8f;
    public float spring = 4.5f;
    public float damper = 7f;
    public float massScale = 4.5f;
    public LayerMask cutterLayer;
    public float tapeSpentMultiplier = 1;
    public GameObject cuttOffTape;
    public GameObject tapedImage;

}
