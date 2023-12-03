using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SledController : MonoBehaviour
{
    public SledPhysics sledPhysics;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) sledPhysics.Respawn();

        sledPhysics.stopThrottleSide = Input.GetAxis("Horizontal");
        sledPhysics.stopThrottle = Input.GetAxis("Jump");
    }
}
