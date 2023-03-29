using _Core;
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

namespace LivingObjects.Herbivore.GameLogic
{
    public class Herbivore : LivingBody
    {
        protected override void Update()
        {
            base.Update();

            if (CurrentHungerLevel > LivingBodyAttributes.HungerLimitToDeath)
            {
                var gameManager = GameManager.Instance(out var isNull);
                if (isNull)
                {
                    return;
                }
                gameManager.ObjectSpawner.DestroyHerbivore(gameObject);
            }
        }

        protected override void CheckSurroundingsCalculateMovement()
        {
            base.CheckSurroundingsCalculateMovement();
            Steering = Vector3.zero;

            int separationCount = 0;
            int alignmentCount = 0;
            int cohesionCount = 0;

            float noClumpingRadius = LivingBodyAttributes.DetectionRad / 4;

            Vector3 seperation = Vector3.zero;
            Vector3 alignement = Vector3.zero;
            Vector3 cohesion = Vector3.zero;

            foreach (var herbivore in LivingThingsAround)
            {
                if (herbivore.CompareTag("Herbivore") && !GameObject.ReferenceEquals(herbivore.gameObject, this.gameObject)) 
                {
                    Vector3 posHerb = herbivore.transform.position;
                    
                    if(Vector3.Magnitude(transform.position - posHerb) < noClumpingRadius)
                    {
                        seperation += herbivore.transform.position - transform.position;
                        separationCount++;
                    }

                    alignement += transform.forward;
                    alignmentCount++;

                    cohesion += herbivore.transform.position - transform.position;
                    cohesionCount++;

                    Debug.DrawLine(transform.position, herbivore.transform.position);
                }   
            }


            //calculate average
            if (separationCount > 0)
            {
                seperation /= separationCount;
            }

            if(alignmentCount > 0)
            {
                alignement /= alignmentCount;
            }

            if (cohesionCount > 0) 
            { 
                cohesion /= cohesionCount;
            }

            //flip and normalize
            seperation = -seperation;

            //get direction to center of mass
            cohesion -= transform.position;

            //weighted rules
            Steering += new Vector3(seperation.x, 0.0f, seperation.z).normalized;
            Steering += new Vector3(alignement.x, 0.0f, alignement.z).normalized;
            Steering += new Vector3(cohesion.x, 0.0f, cohesion.z).normalized;
        }
    }
}


