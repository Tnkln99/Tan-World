using _Core;
using Models.ScriptableObjectModels;
using UnityEngine;


namespace LivingObjects
{
    public class LivingBody : MonoBehaviour
    {
        private Rigidbody _rb;
        private Vector3 _steering;
        private bool _isGameManagerSet;
        protected Collider[] livingThingsAround;
        protected float currentHungerLevel = 0;
        protected GameManager gameManager;
        [SerializeField] protected LivingBodyAttributes LivingBodyAttributes;
        protected virtual void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _steering = Vector3.zero;
        }

        protected virtual void Update()
        {
            if (!_isGameManagerSet)
            {
                gameManager = GameManager.Instance(out var isNull);
                if (isNull)
                {
                    return;
                }

                _isGameManagerSet = true;
            }
            CheckSurroundingsCalculateMovement();
            currentHungerLevel += 0.0f; // todo..
        }

        protected virtual void FixedUpdate()
        {
            var transform1 = transform;
            //apply steering
            if (_steering != Vector3.zero)
            {
                transform1.rotation = Quaternion.RotateTowards(transform1.rotation, Quaternion.LookRotation(_steering), LivingBodyAttributes.SteeringSpeed * Time.deltaTime);
            }
            
            //move 
            transform1.position += transform.TransformDirection(new Vector3(0, 0, LivingBodyAttributes.Speed)) * Time.deltaTime;
        }
           
        protected virtual void CheckSurroundingsCalculateMovement()
        {
            livingThingsAround = Physics.OverlapSphere(transform.position, LivingBodyAttributes.LocalAreaRadious);
            if (!LivingBodyAttributes.MakesFlock)
            {
                return;
            }
            _steering = Vector3.zero;

            int separationCount = 0;
            int alignmentCount = 0;
            int cohesionCount = 0;

            float noClumpingRadius = LivingBodyAttributes.SeparationRadious;

            Vector3 separation = Vector3.zero;
            Vector3 alignment = Vector3.zero;
            Vector3 cohesion = Vector3.zero;

            foreach (var herbivore in livingThingsAround)
            {
                if (herbivore.CompareTag(tag) && !ReferenceEquals(herbivore.gameObject, gameObject)) 
                {
                    Vector3 posHerb = herbivore.transform.position;
                    
                    if(Vector3.Magnitude(transform.position - posHerb) < noClumpingRadius)
                    {
                        separation += herbivore.transform.position - transform.position;
                        separationCount++;
                    }

                    var transform1 = transform;
                    alignment += transform1.forward;
                    alignmentCount++;

                    var position = transform1.position;
                    cohesion += herbivore.transform.position - position;
                    cohesionCount++;

                    Debug.DrawLine(position, posHerb);
                }   
            }


            //calculate average
            if (separationCount > 0)
            {
                separation /= separationCount;
            }

            if(alignmentCount > 0)
            {
                alignment /= alignmentCount;
            }

            if (cohesionCount > 0) 
            { 
                cohesion /= cohesionCount;
            }

            //flip and normalize
            separation = -separation;

            //get direction to center of mass
            cohesion -= transform.position;

            
            SetSteering(separation, alignment, cohesion);
        }

        public void SetSteering(Vector3 separation, Vector3 alignment, Vector3 cohesion)
        {
            //weighted rules
            _steering += new Vector3(separation.x, 0.0f, separation.z).normalized;
            _steering += new Vector3(alignment.x, 0.0f, alignment.z).normalized;
            _steering += new Vector3(cohesion.x, 0.0f, cohesion.z).normalized;
        }

        // for debugging
        private void OnDrawGizmosSelected()
        {
            // Debug to see the detection radious
            var transform1 = transform;
            var pos = transform1.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(pos, LivingBodyAttributes.LocalAreaRadious);
            Debug.DrawLine(pos, (_rb.position + transform1.TransformDirection(_steering.normalized) * LivingBodyAttributes.Speed), Color.blue);
        }
    }
}
