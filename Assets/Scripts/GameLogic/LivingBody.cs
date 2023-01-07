using UnityEngine;

namespace GameLogic
{
    public class LivingBody : MonoBehaviour
    {
        public float Speed = 10;
    
        private Rigidbody _rb;
        private AiBehavior _behavior;
    
        private Vector3 _moveDir;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            if (CompareTag("Herbivore"))
            {
                _behavior = GetComponent<AiHerbivore>();
            }
        
        }

        private void Update()
        {
            _behavior.ChangeBehavior(ref _moveDir);
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + transform.TransformDirection(_moveDir) * (Speed * Time.deltaTime));
        }
    }
}
