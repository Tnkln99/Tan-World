using UnityEngine;

namespace Gravity
{
    public class GravityBody : MonoBehaviour
    {
        public GravityAttracter planet;

        private Transform _myTransform;

        private void Start()
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            GetComponent<Rigidbody>().useGravity = false;
            _myTransform = transform;
        }

        private void Update()
        {
            planet.Attract(_myTransform);
        }
    }
}
