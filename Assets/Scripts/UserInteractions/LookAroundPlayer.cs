using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundPlayer : MonoBehaviour
{
    public float rotationSpeed = 10f;
    //Drag the camera object here
    public Camera cam;  

    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
        float rotY = Input.GetAxis("Mouse Y") * rotationSpeed;

        Vector3 right = Vector3.Cross(cam.transform.up, transform.position - cam.transform.position);
        Vector3 up = Vector3.Cross(transform.position - cam.transform.position, right);
        transform.rotation = Quaternion.AngleAxis(-rotX, up) * transform.rotation;
        transform.rotation = Quaternion.AngleAxis(rotY, right) * transform.rotation;
    }
}
