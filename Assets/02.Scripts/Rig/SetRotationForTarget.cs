using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotationForTarget : MonoBehaviour
{
    [SerializeField] Transform target;

    private void FixedUpdate()
    {
        Vector3 dir = transform.position - target.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, dir);
    }
}
