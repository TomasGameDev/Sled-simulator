using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform cameraPivot;
    public float moveSpeed;
    public Transform target;
    private void Update()
    {
        cameraPivot.position = Vector3.Lerp(cameraPivot.position, target.position, moveSpeed);
    }
}
