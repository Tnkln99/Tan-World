using Models.ScriptableObjectModels;
using UnityEngine;
using Random=UnityEngine.Random;


namespace LivingObjects
{
    public class LivingBody : MonoBehaviour
    {
        protected enum State
        {
            Wandering,
            ChasingFood,
            RunningAway
        }
        
        private Rigidbody _rb;
        private float _wanderingDirChangeTimer;
        private Vector3 _moveDir;
        private float _baseSpeed;
        
        protected Transform EatTarget;
        protected Transform RunAwayTarget;
        protected Collider[] RangeCollider;
        protected bool HasAggro = false;
        protected double HungerLevel;
        protected State state = State.Wandering;
        protected float ReturnWanderTimer;
        [SerializeField] protected LivingBodyAttributes LivingBodyAttributes;

        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _wanderingDirChangeTimer = Time.deltaTime;
            ReturnWanderTimer = Time.deltaTime;

            _baseSpeed = LivingBodyAttributes.Speed;
        }

        protected virtual void Update()
        {
            CheckSurroundings();
            HungerLevel += 0.002;
            switch (state)
            {
                case State.Wandering:
                    Wandering(ref _moveDir);
                    break;
                case State.ChasingFood:
                    ChasingFood(ref _moveDir);
                    break;
                case State.RunningAway:
                    RunningAway(ref _moveDir);
                    break;
            }
        }

        protected virtual void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + transform.TransformDirection(_moveDir) * (LivingBodyAttributes.Speed * Time.deltaTime));
            Debug.DrawLine(transform.position, (_rb.position + transform.TransformDirection(_moveDir) * LivingBodyAttributes.Speed), Color.blue);
        }

        protected virtual void Wandering(ref Vector3 direction)
        {
            LivingBodyAttributes.Speed = _baseSpeed;
            var currentTime = Time.time;
            if (currentTime - _wanderingDirChangeTimer > 4)
            {
                direction = new Vector3(Random.Range(-1f, 1f), 0.0f,Random.Range(-1f, 1f));
                _wanderingDirChangeTimer = currentTime;
            }
        }

        protected virtual void ChasingFood(ref Vector3 direction)
        {
            
        }

        protected virtual void RunningAway(ref Vector3 direction)
        {
            
        }
        
        // this will change the state based on surroundings
        protected virtual void CheckSurroundings()
        {
            RangeCollider = Physics.OverlapSphere(transform.position, LivingBodyAttributes.DetectionRad);
        }

    }
}
