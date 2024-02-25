using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBodyPosition : MonoBehaviour
{
    [SerializeField] Transform target;
    private void FixedUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
