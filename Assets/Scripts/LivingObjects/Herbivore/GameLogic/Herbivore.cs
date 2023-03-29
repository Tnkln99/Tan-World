using _Core;
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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

        protected override void CheckSurroundingsCalculateSteering()
        {
            base.CheckSurroundingsCalculateSteering();
            Steering = Vector3.zero;

            int separationCount = 0;
            int alignmentCount = 0;
            int cohesionCount = 0;

            float noClumpingRadius = LivingBodyAttributes.SeparationRadious;

            Vector3 seperation = Vector3.zero;
            Vector3 alignement = Vector3.zero;
            Vector3 cohesion = Vector3.zero;

            var leaderBoid = Surroundings[0];
            var leaderAngle = 180f;

            foreach (var unit in Surroundings)
            {
                if (unit.CompareTag("Herbivore") && !GameObject.ReferenceEquals(unit.gameObject, this.gameObject)) 
                {
                    Vector3 posHerb = unit.transform.position;
                    
                    if(Vector3.Magnitude(transform.position - posHerb) < noClumpingRadius)
                    {
                        seperation += unit.transform.position - transform.position;
                        separationCount++;
                    }

                    alignement += transform.forward;
                    alignmentCount++;

                    cohesion += unit.transform.position - transform.position;
                    cohesionCount++;

                    var angle = Vector3.Angle(unit.transform.position - transform.position, transform.forward);
                    if (angle < leaderAngle && angle < 90f)
                    {
                        leaderBoid = unit;
                        leaderAngle = angle;
                    }

                    Debug.DrawLine(transform.position, unit.transform.position);
                }   
            }


            //calculate average
            if (separationCount > 0)
            {
                seperation /= separationCount;
            }

            //flip and normalize
            seperation = -seperation;

            if (alignmentCount > 0)
            {
                alignement /= alignmentCount;
            }

            if (cohesionCount > 0) 
            { 
                cohesion /= cohesionCount;
            }

            //get direction to center of mass
            cohesion -= transform.position;

            //weighted rules
            Steering += new Vector3(seperation.x, 0.0f, seperation.z).normalized * 0.5f;
            Steering += new Vector3(alignement.x, 0.0f, alignement.z).normalized * 0.34f;
            Steering += new Vector3(cohesion.x, 0.0f, cohesion.z).normalized * 0.16f;

            if (leaderBoid != null)
            {
                Steering += (leaderBoid.transform.position - transform.position).normalized * 0.5f;
            }
        }
    }
}


