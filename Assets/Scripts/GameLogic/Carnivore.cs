using UnityEngine;

namespace GameLogic
{
    public class Carnivore : LivingBody
    {
        protected override void Update() 
        {
            base.Update();

            if (HungerLevel > hungerLimitToDeath)
            {
                var gameManager = GameManager.Instance(out var isNull);
                if (isNull)
                {
                    return;
                }
                gameManager.ObjectSpawner.DestroyCarnivore(gameObject);
            }
        }
        
        protected override void ChasingFood(ref Vector3 direction)
        {
            base.ChasingFood(ref direction);

            Vector3 selfPos = transform.position;
            Vector3 eatTargetPos = EatTarget.position;
            if (Vector3.Distance(selfPos, eatTargetPos) > 5.0f)
            {
                direction = (new Vector3(eatTargetPos.x - selfPos.x, 0.0f, eatTargetPos.z - selfPos.z)).normalized;
            }
            // this means animal reached to the distance to the plant to be able to eat it
            else
            {
                // Todo: send a message to Herbivore to decrease its life with event system
                HungerLevel = 0;
                direction = new Vector3(0,0,0);
            }
            
            Debug.Log("Carnivore chasing");
        }
        
        protected override void CheckSurroundings()
        {
            base.CheckSurroundings();
            
            foreach (var unit in RangeCollider)
            {
                if (HasAggro)
                {
                    return;
                }
                    
                if (unit.gameObject.CompareTag("Herbivore") /*&& HungerLevel > hungerLimitToLookForFood*/)
                {
                    state = State.ChasingFood;
                    EatTarget = unit.gameObject.transform;
                    HasAggro = true;
                    return;
                }
            }

            state = State.Wandering;
            HasAggro = false;
        }
    }

}