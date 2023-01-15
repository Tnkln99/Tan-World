using System;
using UnityEngine;
using Random=UnityEngine.Random;


namespace GameLogic
{
    public class LivingBody : MonoBehaviour
    {
        protected enum State
        {
            Wondering,
            ChasingFood,
            RunningAway
        }
        
        private Rigidbody _rb;
        private float _wonderingDirChangeTimer;
        private Vector3 _moveDir;
        private float _baseSpeed;
        
        protected Transform EatTarget;
        protected Transform RunAwayTarget;
        protected Collider[] RangeCollider;
        protected bool HasAggro = false;
        protected double HungerLevel = 0;
        protected State state = State.Wondering;
        protected float ReturnWonderTimer;
        
        [SerializeField] protected float speed = 10;
        [SerializeField] protected float detectionRad = 10.0f;
        [SerializeField] protected float hungerLimitToLookForFood = 0.0f;
        [SerializeField] protected float hungerLimitToDeath = Mathf.Infinity;

        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _wonderingDirChangeTimer = Time.deltaTime;
            ReturnWonderTimer = Time.deltaTime;

            _baseSpeed = speed;
        }

        protected virtual void Update()
        {
            CheckSurroundings();
            HungerLevel = HungerLevel + 0.002;
            switch (state)
            {
                case State.Wondering:
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
            _rb.MovePosition(_rb.position + transform.TransformDirection(_moveDir) * (speed * Time.deltaTime));
            Debug.DrawLine(transform.position, (_rb.position + transform.TransformDirection(_moveDir)* speed) ,Color.blue);
        }

        protected virtual void Wandering(ref Vector3 direction)
        {
            speed = _baseSpeed;
            var currentTime = Time.time;
            if (currentTime - _wonderingDirChangeTimer > 4)
            {
                direction = new Vector3(Random.Range(-1f, 1f), 0.0f,Random.Range(-1f, 1f));
                _wonderingDirChangeTimer = currentTime;
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
            RangeCollider = Physics.OverlapSphere(transform.position, detectionRad);
        }

    }
}
