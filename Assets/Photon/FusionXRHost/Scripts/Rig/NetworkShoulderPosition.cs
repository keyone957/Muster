using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkShoulderPosition : MonoBehaviour
{
    [SerializeField] Transform target;
    private void FixedUpdate()
    {
        transform.position = target.position;
    }
}
