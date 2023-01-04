using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.UIElements;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GravityAttracter : MonoBehaviour
{
    public float gravity = -10.0f;
    public void Attract(Transform body)
    {
        Vector3 gravityUp = (body.position - transform.position).normalized;
        Vector3 bodyUp = body.up;
        
        
        body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
    }
}
