using UnityEngine;

namespace UserInteractions
{
    public class LookAroundPlayer : MonoBehaviour
    {
        public float RotationSpeed = 10f;
        //Drag the camera object here
        public Camera Cam;

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
    }
}
