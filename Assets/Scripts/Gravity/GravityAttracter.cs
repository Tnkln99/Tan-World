using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Gravity
{
    public class GravityAttracter : MonoBehaviour
    {
        public float gravity = -10.0f;
        public void Attract(Transform body)
        {
            var gravityUp = (body.position - transform.position).normalized;
            var bodyUp = body.up;
        
        
            body.GetComponent<Rigidbody>().AddForce(gravityUp * gravity);
            var rotation = body.rotation;
            var targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * rotation;
            rotation = Quaternion.Slerp(rotation, targetRotation, 50 * Time.deltaTime);
            body.rotation = rotation;
        }
    }
}
