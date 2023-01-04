using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public GravityAttracter planet;

    private Transform _myTransform;
    void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        GetComponent<Rigidbody>().useGravity = false;
        _myTransform = transform;
    }
    
    void Update()
    {
        planet.Attract(_myTransform);
    }
}
