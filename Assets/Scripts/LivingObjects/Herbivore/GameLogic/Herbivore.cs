using _Core;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

namespace LivingObjects.Herbivore.GameLogic
{
    public class Herbivore : LivingBody
    {

        private Vector3 _cohesionVector = new Vector3(0,0,0);
        private Vector3 _seperationVector = new Vector3(0,0,0);
        private Vector3 _alignementVector = new Vector3(0,0,0);

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

            Accel += _cohesionVector + _seperationVector + _alignementVector;
        }

        protected override void CheckSurroundings()
        {
            base.CheckSurroundings();

            int numberOfHerbivoreDetected = 0;
            Vector3 herbivorePositionSum = new Vector3 (0,0,0);
            Vector3 seperation = new Vector3 (0,0,0);
            Vector3 herbivoreVelocitySum = new Vector3 (0,0,0);

            foreach (var unit in RangeCollider)
            {
                if (unit.CompareTag("Herbivore") && !GameObject.ReferenceEquals(unit.gameObject, this.gameObject)) 
                {
                    numberOfHerbivoreDetected++;
                    Vector3 posHerb = unit.transform.position;
                    herbivorePositionSum += new Vector3(posHerb.x, 0.0f, posHerb.z);
                    herbivoreVelocitySum += unit.gameObject.GetComponent<Rigidbody>().velocity;

                    if(Vector3.Magnitude(transform.position - posHerb) < LivingBodyAttributes.DetectionRad / 2)
                    {
                        Vector3 difference = transform.position - posHerb;
                        seperation = difference - seperation;
                    }

                    Debug.DrawLine(transform.position, unit.transform.position);
                }   
            }


            if (numberOfHerbivoreDetected > 0)
            {
                _cohesionVector = herbivorePositionSum / numberOfHerbivoreDetected;
                _seperationVector = seperation;
                _alignementVector = herbivoreVelocitySum / numberOfHerbivoreDetected;
            }
        }   
    }
}


