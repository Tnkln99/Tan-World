using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public GameObject herbivore;

    public GameObject planet;

    public GameObject plant;

    public UiManager canvas;

    private int _countHerbivore = 0;
    private int _countPlants = 0;
    

    // Update is called once per frame
    void Update()
    {
        SpawnElement();
    }

    void SpawnElement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if (planet.GetComponent<SphereCollider>().Raycast(ray, out hitData, 1000))
        {
            // Spawns Herbivore
            if (Input.GetMouseButtonDown(1))
            {
                _countHerbivore++;
                GameObject newHerb = Instantiate(herbivore, planet.transform, false);
                newHerb.GetComponent<GravityBody>().planet = planet.GetComponent<GravityAttracter>();
                newHerb.transform.position = hitData.point;
                newHerb.transform.localScale = new Vector3(0.008f, 0.008f, 0.008f);
                canvas.UpdateHerbivoreCount(_countHerbivore);
            }
            // Spawns Plant
            if (Input.GetMouseButtonDown(2))
            {
                _countPlants++;
                GameObject newPlant = Instantiate(plant, planet.transform, false);
                newPlant.GetComponent<GravityBody>().planet = planet.GetComponent<GravityAttracter>();
                newPlant.GetComponent<Plant>().gameManager = this;
                newPlant.transform.position = hitData.point;
                newPlant.transform.localScale = new Vector3(0.0128334f, 0.03411889f, 0.0128334f);
                canvas.UpdatePlantCount(_countPlants);
            }
        }
    }

    public void DeadPlant()
    {
        _countPlants--;
        canvas.UpdatePlantCount(_countPlants);
    }
}
