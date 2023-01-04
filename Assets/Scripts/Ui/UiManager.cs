using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI herbivoreCount;
    public TextMeshProUGUI plantCount;

    public void UpdateHerbivoreCount(int count)
    {
        herbivoreCount.text = "Herbivore Count: " + count.ToString();
    }
    
    public void UpdatePlantCount(int count)
    {
        plantCount.text = "Plant Count: " + count.ToString();
    }
}
