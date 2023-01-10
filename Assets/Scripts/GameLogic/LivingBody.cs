using UnityEngine;

namespace GameLogic
{
    public class LivingBody : MonoBehaviour
    {
        public float Speed = 10;
    
        private Rigidbody _rb;

        protected Vector3 _moveDir;

        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody>();

        }

        protected virtual void Update()
        {
            
        }

        protected virtual void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + transform.TransformDirection(_moveDir) * (Speed * Time.deltaTime));
        }
    }
}
