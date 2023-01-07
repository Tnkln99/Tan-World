using UnityEngine;
using Random = UnityEngine.Random;

namespace GameLogic
{
    public class AiBehavior : MonoBehaviour
    {
        protected enum State
        {
            Wondering,
            RunningAway,
            Eating,
            Hunting
        }
    
        protected State state = State.Wondering;
    
        private float _stateChangeTimer;

        private void Start()
        {
            _stateChangeTimer = Time.deltaTime;
        }

        public void ChangeBehavior(ref Vector3 direction)
        {
            //todo: remove Debug.Log(state);
            switch (state)
            {
                case State.Wondering:
                    Wandering(ref direction);
                    break;
                case State.Eating:
                    Eating(ref direction);
                    break;
            }
        }

        private void Wandering(ref Vector3 direction)
        {
            var currentTime = Time.time;
            if (currentTime - _stateChangeTimer > 4)
            {
                _stateChangeTimer = currentTime;
                direction = new Vector3(Random.Range(-1f, 1f), 0.0f,Random.Range(-1f, 1f)); 
            }
        }

        protected virtual void Eating(ref Vector3 direction)
        {
            var currentTime = Time.time;
            if (currentTime - _stateChangeTimer > 4)
            {
                _stateChangeTimer = currentTime;
                state = State.Wondering;
            }
        }
    }
}
