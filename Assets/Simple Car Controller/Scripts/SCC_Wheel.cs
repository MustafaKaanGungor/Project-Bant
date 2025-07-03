//----------------------------------------------
//            Simple Car Controller
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handling wheels. Visual and physicaly. Based on Unity's WheelCollider.
/// </summary>
[AddComponentMenu("BoneCracker Games/Simple Car Controller/SCC Wheel")]
[RequireComponent(typeof(WheelCollider))]
public class SCC_Wheel : MonoBehaviour {

    //  Rigidbody of the vehicle.
    private Rigidbody rigid;
    private Rigidbody Rigid {

        get {

            if (rigid == null)
                rigid = GetComponentInParent<Rigidbody>();

            return rigid;

        }

    }

    //  Drivetrain.
    private SCC_Drivetrain drivetrain;
    private SCC_Drivetrain Drivetrain {

        get {

            if (drivetrain == null)
                drivetrain = GetComponentInParent<SCC_Drivetrain>();

            return drivetrain;

        }

    }

    //  Wheelcollider.
    private WheelCollider wheelCollider;
    public WheelCollider WheelCollider {

        get {

            if (wheelCollider == null)
                wheelCollider = GetComponent<WheelCollider>();

            return wheelCollider;

        }

        set {

            wheelCollider = value;

        }

    }

    private float wheelRotation = 0f;       //  Current wheel rotation X axis
    internal bool isGrounded = false;       //  Is this wheel grounded or not?
    internal float totalSlip = 0f;      //  Total slipage.
    internal float rpm = 0f;        //  Current rpm of the wheel.
    internal float wheelRPMToSpeed = 0f;        //  RPM to speed converstaion.

    private void Awake() {

    }

    private void Update() {

        //  If there is no drivetrain or wheelcollider, return.
        if (!Drivetrain || !WheelCollider) {

            Debug.LogError("Drivetrain or wheelcollider is not found. This wheel is disabled");
            enabled = false;
            return;

        }

        WheelAlign();

    }

    private void FixedUpdate() {

        WheelHit hit;
        isGrounded = WheelCollider.GetGroundHit(out hit);

        rpm = WheelCollider.rpm;        //  RPM of the wheel.
        wheelRPMToSpeed = (((WheelCollider.rpm * WheelCollider.radius) / 2.8f) * Mathf.Lerp(1f, .75f, hit.forwardSlip)) * Rigid.transform.lossyScale.y;     //  RPM to speed.

    }

    /// <summary>
    /// Aligning wheel models.
    /// </summary>
    private void WheelAlign() {



        RaycastHit hit;
        WheelHit CorrespondingGroundHit;

        Vector3 ColliderCenterPoint = WheelCollider.transform.TransformPoint(WheelCollider.center);
        WheelCollider.GetGroundHit(out CorrespondingGroundHit);

        if (Physics.Raycast(ColliderCenterPoint, -WheelCollider.transform.up, out hit, (WheelCollider.suspensionDistance + WheelCollider.radius) * transform.localScale.y) && !hit.collider.isTrigger && !hit.transform.IsChildOf(Rigid.transform)) {

            
            float extension = (-WheelCollider.transform.InverseTransformPoint(CorrespondingGroundHit.point).y - WheelCollider.radius) / WheelCollider.suspensionDistance;
            Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point + WheelCollider.transform.up * (CorrespondingGroundHit.force / Rigid.mass), extension <= 0.0 ? Color.magenta : Color.white);
            Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - WheelCollider.transform.forward * CorrespondingGroundHit.forwardSlip * 2f, Color.green);
            Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - WheelCollider.transform.right * CorrespondingGroundHit.sidewaysSlip * 2f, Color.red);

        } else {


        }

        wheelRotation += WheelCollider.rpm * 6 * Time.deltaTime;
        
    }

}