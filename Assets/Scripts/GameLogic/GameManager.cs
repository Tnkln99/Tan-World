using System;
using Gravity;
using Ui;
using UnityEngine;

namespace GameLogic
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        
        public GameObject herbivore;

        public GameObject planet;

        public GameObject plant;

        public UiManager canvas;

        private int _countHerbivore;
        private int _countPlants;

        public static GameManager Instance(out bool isNull)
        {
            if (_instance == null)
            {
                isNull = true;
                return null;
            }
            isNull = false;
            return _instance;
        }

        private void Awake()
        {
            _instance = this;
        }

        // Update is called once per frame
        private void Update()
        {
            SpawnElement();
        }

        private void SpawnElement()
        {
            if (Camera.main == null)
            {
                return;
            }
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (planet.GetComponent<SphereCollider>().Raycast(ray, out var hitData, 1000))
            {
                // Spawns Herbivore
                if (Input.GetMouseButtonDown(1))
                {
                    _countHerbivore++;
                    var newHerb = Instantiate(herbivore, null);
                    newHerb.transform.SetParent(planet.transform, false);
                    newHerb.GetComponent<GravityBody>().planet = planet.GetComponent<GravityAttracter>();
                    newHerb.transform.position = hitData.point;
                    canvas.UpdateHerbivoreCount(_countHerbivore);
                }
                // Spawns Plant
                if (Input.GetMouseButtonDown(2))
                {
                    _countPlants++;
                    var newPlant = Instantiate(plant, null);
                    newPlant.transform.SetParent(planet.transform, false);
                    newPlant.GetComponent<GravityBody>().planet = planet.GetComponent<GravityAttracter>();
                    newPlant.GetComponent<Plant>().gameManager = this;
                    newPlant.transform.position = hitData.point;
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
}
