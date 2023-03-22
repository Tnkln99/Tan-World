using _Core;
using UnityEngine;

namespace LivingObjects.Carnivore.GameLogic
{
    public class Carnivore : LivingBody
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
                gameManager.ObjectSpawner.DestroyCarnivore(gameObject);
            }
        }
        
        protected override void CheckSurroundings()
        {
            base.CheckSurroundings();
            
            foreach (var unit in RangeCollider)
            {
               
            }
        }
    }

}