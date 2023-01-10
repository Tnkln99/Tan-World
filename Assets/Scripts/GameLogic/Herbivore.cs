using System.Collections;
using System.Collections.Generic;
using GameLogic;
using UnityEngine;

namespace GameLogic
{
    public class Herbivore : LivingBody
    {
        enum State
        {
            Wondering,
            Eating
        }
    
        private State state = State.Wondering;
        
        private float _wonderingDirChangeTimer;
        private float _returnWonderTimer; // todo: this is temporary
        
        protected override void Start()
        {
            base.Start();
            _wonderingDirChangeTimer = Time.deltaTime;
            _returnWonderTimer = Time.deltaTime;
        }
        
        protected override void Update() 
        {
            base.Update();
            switch (state)
            {
                case State.Wondering:
                    Wandering(ref _moveDir);
                    break;
                case State.Eating:
                    Eating(ref _moveDir);
                    break;
            }
        }
    
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        
        private void ChangeBehavior(ref Vector3 direction)
        {
            
        }
        
        private void Wandering(ref Vector3 direction)
        {
            var currentTime = Time.time;
            if (currentTime - _wonderingDirChangeTimer > 4)
            {
                direction = new Vector3(Random.Range(-1f, 1f), 0.0f,Random.Range(-1f, 1f));
                _wonderingDirChangeTimer = currentTime;
            }
        }
    
        protected virtual void Eating(ref Vector3 direction)
        {
            var currentTime = Time.time;
            if (currentTime - _returnWonderTimer > 2)
            {
                _returnWonderTimer = currentTime;
                state = State.Wondering;
            }
            direction = new Vector3(0, 0,  0);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 3)
            {
                return;
            }
    
            if (other.CompareTag("Plant"))
            {
                state = State.Eating;
                other.gameObject.GetComponent<Plant>().DecreaseLife();
            }
        }
    }
}


