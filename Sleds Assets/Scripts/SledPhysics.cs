using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SledPhysics : MonoBehaviour
{
    public float forwardSpeed;
    public float forwardSpeedAbs;

    public float sideSpeed;
    public float sideSpeedAbs;

    Vector3 forwardPoint;
    Vector3 sidePoint;
    public float stopThrottleSide;
    public float stopThrottle;
    public float stopForce = 1;
    public float maxStopMagnitude = 1.5f;
    public float stopThrottleR;
    public float stopThrottleL;
    public float sideFriction;
    public float sideAcceleration;
    public float equalization;
    public Rigidbody rig;
    public bool isGrounded; 

    void FixedUpdate()
    {
        //FORWARD SPEED
        Vector3 _forwardPoint = transform.position + Vector3.Project(forwardPoint - transform.position, transform.forward) + Vector3.Dot(Vector3.zero, transform.forward) * transform.forward;
        Vector3 _forwardPointNormal = transform.position - _forwardPoint;
        forwardSpeed = _forwardPointNormal.magnitude;
        forwardSpeedAbs = forwardSpeed /= 2f;
        bool forwardPointDirection = Vector3.Distance(transform.forward, _forwardPointNormal) < Vector3.Distance(-transform.forward, _forwardPointNormal);
        forwardSpeed *= forwardPointDirection ? 1f : -1f;
        forwardPoint = transform.position;
        //SIDE SPEED

        Vector3 _sidePoint = transform.position + Vector3.Project(sidePoint - transform.position, transform.right) + Vector3.Dot(Vector3.zero, transform.forward) * transform.right;
        Vector3 _sidePointNormal = transform.position - _sidePoint;
        sideSpeed = _sidePointNormal.magnitude;
        sideSpeedAbs = sideSpeed /= 2f;
        bool sidePointDirection = Vector3.Distance(transform.forward, _sidePointNormal) < Vector3.Distance(-transform.right, _sidePointNormal);
        sideSpeed *= sidePointDirection ? 1f : -1f;
        sidePoint = transform.position;


        if (stopThrottleSide != 0 || stopThrottle > 0)
        {
            if (stopThrottle == 0)
            {
                stopThrottleR = stopThrottleSide > 0 ? stopThrottleSide : 0;
                stopThrottleL = stopThrottleSide < 0 ? -stopThrottleSide : 0;
            }
            else
            {
                stopThrottleR = stopThrottleL = stopThrottle;
            }
            Ray rayR = new Ray();
            Ray rayL = new Ray();
            rayR.origin = LegRPoint() + transform.up * 0.25f;
            rayL.origin = LegLPoint() + transform.up * 0.25f;
            rayR.direction = rayL.direction = -transform.up;
            if (Physics.Raycast(rayR, 0.5f))
            {
                Vector3 stopVelocityR = stopForce * -transform.forward * forwardSpeedAbs * stopThrottleR;
                stopVelocityR = Vector3.ClampMagnitude(stopVelocityR, maxStopMagnitude);
                rig.AddForceAtPosition(stopVelocityR, LegRPoint());
                if (stopThrottleR > 0) Debug.DrawRay(rayR.origin, rayR.direction * 0.5f, Color.yellow);
            }
            if (Physics.Raycast(rayL, 0.5f))
            {
                Vector3 stopVelocityL = stopForce * -transform.forward * forwardSpeedAbs * stopThrottleL;
                stopVelocityL = Vector3.ClampMagnitude(stopVelocityL, maxStopMagnitude);
                rig.AddForceAtPosition(stopVelocityL, LegLPoint());
                if (stopThrottleL > 0) Debug.DrawRay(rayL.origin, rayL.direction * 0.5f, Color.yellow);
            }
        }
        else
        {
            stopThrottleR = stopThrottleL = 0;
        }

        if (isGrounded)
        {
            Vector3 forwardVelocity = transform.forward * sideSpeed;
            Vector3 sideVelocity = -transform.right * sideSpeed;
            rig.velocity += forwardVelocity - transform.right * sideSpeed * sideAcceleration;
            Ray rayR = new Ray();
            Ray rayL = new Ray();
            rayR.origin = RailRPoint() + transform.up * 0.25f;
            rayL.origin = RailLPoint() + transform.up * 0.25f;
            rayR.direction = rayL.direction = -transform.up;
            if (Physics.Raycast(rayR, 0.5f) || Physics.Raycast(rayL, 0.5f))
            {
                rig.AddForceAtPosition(transform.up * sideSpeed * sideFriction, sideSpeed > 0 ? RailLPoint() : RailRPoint());
                rig.AddForceAtPosition(sideVelocity * equalization, CenterOfMassPoint());
            }
            Debug.DrawRay(rayR.origin, rayR.direction * 0.5f, Color.red);
            Debug.DrawRay(rayL.origin, rayL.direction * 0.5f, Color.red);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    public Vector3 spawnPoint;
    public float spawnY;
    public void Respawn()
    {
        transform.position = spawnPoint;
        transform.rotation = Quaternion.Euler(0, spawnY, 0);
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        forwardSpeed = forwardSpeedAbs = 0;
        sideSpeed = sideSpeedAbs = 0;
        forwardPoint = transform.position;
        sidePoint = transform.position;
    }
    public Vector3 legR;
    public Vector3 legL;

    public Vector3 railR;
    public Vector3 railL;

    public Vector3 centerOfMass;
    Vector3 LegRPoint()
    {
        return transform.position + transform.forward * legR.z + transform.right * legR.x + transform.up * legR.y;
    }
    Vector3 LegLPoint()
    {
        return transform.position + transform.forward * legL.z + transform.right * legL.x + transform.up * legL.y;
    }

    Vector3 RailRPoint()
    {
        return transform.position + transform.forward * railR.z + transform.right * railR.x + transform.up * railR.y;
    }
    Vector3 RailLPoint()
    {
        return transform.position + transform.forward * railL.z + transform.right * railL.x + transform.up * railL.y;
    }
    Vector3 CenterOfMassPoint()
    {
        return transform.position + transform.forward * centerOfMass.z + transform.right * centerOfMass.x + transform.up * centerOfMass.y;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(LegRPoint(), Vector3.one * 0.05f);
        Gizmos.DrawWireCube(LegLPoint(), Vector3.one * 0.05f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.forward * 0.5f + RailRPoint(), -transform.forward * 0.5f + RailRPoint());
        Gizmos.DrawLine(transform.forward * 0.5f + RailLPoint(), -transform.forward * 0.5f + RailLPoint());

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(CenterOfMassPoint(), Vector3.one * 0.2f);
    }
}
