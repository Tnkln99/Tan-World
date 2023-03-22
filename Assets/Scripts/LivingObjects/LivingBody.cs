using Models.ScriptableObjectModels;
using UnityEngine;
using Random=UnityEngine.Random;


namespace LivingObjects
{
    public class LivingBody : MonoBehaviour
    { 
        
        private Rigidbody _rb;
        private float _baseSpeed;
        
        protected Vector3 Velocity;
        protected Vector3 Accel;
        protected Collider[] RangeCollider;
        protected float CurrentHungerLevel = 0;
        [SerializeField] protected LivingBodyAttributes LivingBodyAttributes;

        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody>();

            _baseSpeed = LivingBodyAttributes.Speed;

            Velocity = new Vector3(Random.Range(-1f, 1f), 0.0f, Random.Range(-1f, 1f)).normalized;
            Accel = Vector3.zero;
        }

        protected virtual void Update()
        {
            CheckSurroundings();
            CurrentHungerLevel += 0.0f; // todo..

            Velocity += Accel;
            Velocity = Velocity.normalized;
            Vector3.ClampMagnitude(Velocity, _baseSpeed);
            Accel = Vector3.zero;
        }

        // for debugging
        private void OnDrawGizmosSelected()
        {
            // Debug to see the detection radious
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, LivingBodyAttributes.DetectionRad);

            Debug.DrawLine(transform.position, (_rb.position + transform.TransformDirection(Velocity) * LivingBodyAttributes.Speed), Color.blue);
        }

        protected virtual void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + transform.TransformDirection(Velocity) * (LivingBodyAttributes.Speed * Time.deltaTime));
        }
        
        // this will change the state based on surroundings
        protected virtual void CheckSurroundings()
        {
            RangeCollider = Physics.OverlapSphere(transform.position, LivingBodyAttributes.DetectionRad);
        }

    }
}
