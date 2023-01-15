using _Core;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace LivingObjects.Herbivore.GameLogic
{
    public class Herbivore : LivingBody
    {
        protected override void Update() 
        {
            base.Update();

            if (HungerLevel > LivingBodyAttributes.HungerLimitToDeath)
            {
                var gameManager = GameManager.Instance(out var isNull);
                if (isNull)
                {
                    return;
                }
                gameManager.ObjectSpawner.DestroyHerbivore(gameObject);
            }
        }

        protected override void ChasingFood(ref Vector3 direction)
        {
            base.ChasingFood(ref direction);
            
            var currentTime = Time.time;
            if (currentTime - ReturnWanderTimer > 10)
            {
                ReturnWanderTimer = currentTime;
                state = State.Wandering;
                HasAggro = false;
            }
            
            Vector3 selfPos = transform.position;
            Vector3 eatTargetPos = EatTarget.position;
            if (Vector3.Distance(selfPos, eatTargetPos) > 5.0f)
            {
                direction = (new Vector3(eatTargetPos.x - selfPos.x, 0.0f, eatTargetPos.z - selfPos.z)).normalized;
            }
            // this means animal reached to the distance to the plant to be able to eat it
            else
            {
                // Todo: send a message to plant to decrease its life with event system
                HungerLevel = 0;
                direction = new Vector3(0,0,0);
            }

            Debug.Log("chasing food");
        }

        protected override void RunningAway(ref Vector3 direction)
        {
            base.RunningAway(ref direction);

            Vector3 selfPos = transform.position;
            Vector3 runAwayTargetPos = RunAwayTarget.position;
            
            direction = (new Vector3(selfPos.x - runAwayTargetPos.x, 0.0f, selfPos.z - runAwayTargetPos.z)).normalized;

            Debug.Log("Herbivore running away");
        }

        protected override void CheckSurroundings()
        {
            base.CheckSurroundings();
            
            foreach (var unit in RangeCollider)
            {
                if (unit.gameObject.CompareTag("Carnivore"))
                {
                    RunAwayTarget = unit.gameObject.transform;
                    state = State.RunningAway;
                    return;
                }
                
                if (unit.gameObject.CompareTag("Plant") && !HasAggro && HungerLevel > LivingBodyAttributes.HungerLimitToLookForFood)
                {
                    ReturnWanderTimer = Time.time;
                    state = State.ChasingFood;
                    EatTarget = unit.gameObject.transform;
                    HasAggro = true;
                }
            }
        }
    }
}


