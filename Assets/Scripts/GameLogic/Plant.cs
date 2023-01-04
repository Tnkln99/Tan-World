using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private int _live = 5;

    public GameManager gameManager;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void DecreaseLive()
    {
        _live--;
        if (_live <= 0)
        {
            gameManager.DeadPlant();
            Destroy(gameObject);
        }
    }
}
