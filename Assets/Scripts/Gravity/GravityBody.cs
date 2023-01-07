using UnityEngine;

namespace Gravity
{
    public class GravityBody : MonoBehaviour
    {
        public GravityAttracter Planet;

        private Transform _myTransform;

        private void Start()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            GetComponent<Rigidbody>().useGravity = false;
            _myTransform = transform;
        }

        private void Update()
        {
            Planet.Attract(_myTransform);
        }
    }
}
