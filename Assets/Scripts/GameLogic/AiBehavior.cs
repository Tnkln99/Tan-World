using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        //Debug.Log(state);
        switch (state)
        {
            case State.Wondering:
                Wondering(ref direction);
                break;
            case State.Eating:
                Eating(ref direction);
                break;
        }
    }

    private void Wondering(ref Vector3 direction)
    {
        float currtime = Time.time;
        if (currtime - _stateChangeTimer > 4)
        {
            _stateChangeTimer = currtime;
            direction = new Vector3(Random.Range(-1f, 1f), 0.0f,Random.Range(-1f, 1f)); 
        }
    }

    protected virtual void Eating(ref Vector3 direction)
    {
        float currtime = Time.time;
        if (currtime - _stateChangeTimer > 4)
        {
            _stateChangeTimer = currtime;
            state = State.Wondering;
        }
    }
}
