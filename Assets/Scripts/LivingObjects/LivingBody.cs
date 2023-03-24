using Models.ScriptableObjectModels;
using UnityEngine;
using Random=UnityEngine.Random;


namespace LivingObjects
{
    public class LivingBody : MonoBehaviour
    { 
        
        private Rigidbody _rb;
        private float _baseSpeed;
        
        protected Vector3 moveDir;
        protected Vector3 Accel;
        protected Collider[] RangeCollider;
        protected float CurrentHungerLevel = 0;
        [SerializeField] protected LivingBodyAttributes LivingBodyAttributes;

        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody>();

            _baseSpeed = LivingBodyAttributes.Speed;

            moveDir = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f)).normalized;
            Accel = Vector3.zero;
        }

        protected virtual void Update()
        {
            CheckSurroundings();
            CurrentHungerLevel += 0.0f; // todo..

            moveDir += Accel;
            moveDir = moveDir.normalized;
            Accel = Vector3.zero;
        }

        protected virtual void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + transform.TransformDirection(moveDir) * (LivingBodyAttributes.Speed * Time.deltaTime));
        }

        // for debugging
        private void OnDrawGizmosSelected()
        {
            // Debug to see the detection radious
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, LivingBodyAttributes.DetectionRad);

            Debug.DrawLine(transform.position, (_rb.position + transform.TransformDirection(moveDir) * LivingBodyAttributes.Speed), Color.blue);
        }
           
        protected virtual void CheckSurroundings()
        {
            RangeCollider = Physics.OverlapSphere(transform.position, LivingBodyAttributes.DetectionRad);
        }

    }
}
