using _Core;
using UnityEngine;

namespace LivingObjects.Carnivore.GameLogic
{
    public class Carnivore : LivingBody
    {
        protected override void Update() 
        {
            base.Update();

            if (currentHungerLevel > LivingBodyAttributes.HungerLimitToDeath)
            {
                var gameManager = GameManager.Instance(out var isNull);
                if (isNull)
                {
                    return;
                }
                gameManager.ObjectSpawner.DestroyCarnivore(gameObject);
            }
        }
        
        protected override void CheckSurroundingsCalculateMovement()
        {
            base.CheckSurroundingsCalculateMovement();
            
            foreach (var unit in livingThingsAround)
            {
               
            }
        }
    }

}