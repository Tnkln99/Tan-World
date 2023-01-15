using System;
using UnityEngine;

namespace UserInteractions
{
    public class LookAroundPlayer : MonoBehaviour
    {
        public float RotationSpeed = 10f;

        //Drag the camera object here
        public Camera Cam;
        public float zoomLevel;
        public float sensitivity = 1.2f;
        public float speed = 30;
        public float maxZoom = 30;
        float zoomPosition;

        private void OnMouseDrag()
        {
            var rotX = Input.GetAxis("Mouse X") * RotationSpeed;
            var rotY = Input.GetAxis("Mouse Y") * RotationSpeed;
            var camTransform = Cam.transform;
            var transform1 = transform;
            var rotation = transform1.rotation;
            var position = transform1.position;
            var camTransformPosition = camTransform.position;
            var right = Vector3.Cross(camTransform.up, position - camTransformPosition);
            var up = Vector3.Cross(position - camTransformPosition, right);
            rotation = Quaternion.AngleAxis(-rotX, up) * rotation;
            rotation = Quaternion.AngleAxis(rotY, right) * rotation;
            transform1.rotation = rotation;
        }

        private void OnMouseOver()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                var transform1 = transform;
                var position = transform1.position;

                zoomLevel += Input.mouseScrollDelta.y * sensitivity;
                zoomLevel = Mathf.Clamp(zoomLevel, 0, maxZoom);
                zoomPosition = Mathf.MoveTowards(zoomPosition, zoomLevel, speed * Time.deltaTime);
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    transform1.position = position + (transform1.forward * zoomPosition);
                }
                else
                {
                    transform1.position = position - (transform1.forward * zoomPosition);
                }
            }
        }
        
    }
}