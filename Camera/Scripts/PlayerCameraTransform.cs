using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoysPlayer
{
    public class PlayerCameraTransform : MonoBehaviour
{
    [HideInInspector] public float x;
    [HideInInspector] public float z;
    [HideInInspector] public Transform pos;
    void Start()
    {
        pos = transform;
    }
    void Update()
    {
        pos.localRotation = Quaternion.Euler(x, 0, z);
    }
}
}