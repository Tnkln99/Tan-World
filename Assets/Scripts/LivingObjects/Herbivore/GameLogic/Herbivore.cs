using _Core;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace LivingObjects.Herbivore.GameLogic
{
    public class Herbivore : LivingBody
    {

        private Vector3 _cohesionVector = new Vector3(0,0,0);

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

            Accel += _cohesionVector;
        }

        protected override void CheckSurroundings()
        {
            base.CheckSurroundings();
            int numberOfHerbivoreDetected = 0;
            Vector3 herbivorePositionSum = new Vector3 (0,0,0);
            foreach (var unit in RangeCollider)
            {
                if (unit.CompareTag("Herbivore") && !GameObject.ReferenceEquals(unit.gameObject, this.gameObject)) 
                {
                    numberOfHerbivoreDetected++;
                    herbivorePositionSum += unit.transform.position;
                }   
            }


            if (numberOfHerbivoreDetected > 0)
            {
                CalculateCohesionVector(numberOfHerbivoreDetected, herbivorePositionSum);
            }
        }   

        private void CalculateCohesionVector(int numberOfHerbivoreDetected, Vector3 herbivorePositionSum)
        {
            _cohesionVector = herbivorePositionSum / numberOfHerbivoreDetected;
        }
    }
}


