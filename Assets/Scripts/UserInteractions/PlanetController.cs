using _Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInteractions
{
    public class PlanetController : MonoBehaviour, IDragHandler, IPointerClickHandler

    {
        [SerializeField] private float RotationSpeed = 10f;
        [SerializeField] private float Sensitivity = 1.2f;
        [SerializeField] private float Speed = 30;
        [SerializeField] private float MAXZoom = 30;

        private float _zoomLevel;
        private float _zoomPosition;

        private void OnMouseOver()
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                var transform1 = transform;
                var position = transform1.position;

                _zoomLevel += Input.mouseScrollDelta.y * Sensitivity;
                _zoomLevel = Mathf.Clamp(_zoomLevel, 0, MAXZoom);
                _zoomPosition = Mathf.MoveTowards(_zoomPosition, _zoomLevel, Speed * Time.deltaTime);
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    transform1.position = position + (transform1.forward * _zoomPosition);
                }
                else
                {
                    transform1.position = position - (transform1.forward * _zoomPosition);
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            var gameManager = GameManager.Instance(out var isNull);
            if (isNull)
            {
                return;
            }

            var rotX = Input.GetAxis("Mouse X") * RotationSpeed;
            var rotY = Input.GetAxis("Mouse Y") * RotationSpeed;
            var camTransform = gameManager.MainCamera.transform;
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

        public void OnPointerClick(PointerEventData eventData)
        {
            var gameManager = GameManager.Instance(out var isNull);
            if (isNull || eventData.button != PointerEventData.InputButton.Left || eventData.dragging)
            {
                return;
            }

            gameManager.ObjectSpawner.SpawnElement();
        }
    }
}