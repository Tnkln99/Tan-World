using Models.ScriptableObjectModels;
using System;
using UnityEngine;
using Random=UnityEngine.Random;


namespace LivingObjects
{
    public class LivingBody : MonoBehaviour
    {
        private Rigidbody _rb;
        
        protected Vector3 Steering;
        protected Collider[] LivingThingsAround;
        protected float CurrentHungerLevel = 0;
        [SerializeField] protected LivingBodyAttributes LivingBodyAttributes;

        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody>();

            Steering = Vector3.zero;
        }

        protected virtual void Update()
        {
            CheckSurroundingsCalculateMovement();
            CurrentHungerLevel += 0.0f; // todo..
        }

        protected virtual void FixedUpdate()
        {
            //apply steering
            if (Steering != Vector3.zero)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Steering), LivingBodyAttributes.SteeringSpeed * Time.deltaTime);
            }
            
            //move 
            transform.position += transform.TransformDirection(new Vector3(0, 0, LivingBodyAttributes.Speed)) * Time.deltaTime;
        }
           
        protected virtual void CheckSurroundingsCalculateMovement()
        {
            LivingThingsAround = Physics.OverlapSphere(transform.position, LivingBodyAttributes.DetectionRad);
        }

        // for debugging
        private void OnDrawGizmosSelected()
        {
            // Debug to see the detection radious
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, LivingBodyAttributes.DetectionRad);

            Debug.DrawLine(transform.position, (_rb.position + transform.TransformDirection(Steering.normalized) * LivingBodyAttributes.Speed), Color.blue);
        }
    }
}
