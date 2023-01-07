using System.Collections.Generic;
using Gravity;
using Helpers.ObjectPool;
using UnityEngine;

namespace GameLogic
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private ObjectPooler ObjectPooler;
        [SerializeField] private GameObject Herbivore;
        [SerializeField] private GameObject Planet;
        [SerializeField] private GameObject Plant;
        
        private int _countHerbivore;
        private int _countPlants;
        
        // Update is called once per frame
        private void Update()
        {
            SpawnElement();
        }

        private void Awake()
        {
            var pooledTypes = new List<PooledType>();
            pooledTypes.Add(new PooledType()
            {
                AutoExpand = true,
                Max = 1000,
                Prefab = Herbivore,
                SortingTag = "Herbivore",
                TypeName = "Herbivore"
            });
            pooledTypes.Add(new PooledType()
            {
                AutoExpand = true,
                Max = 1000,
                Prefab = Plant,
                SortingTag = "Plant",
                TypeName = "Plant"
            });
            ObjectPooler.Initialize(pooledTypes);
        }

        private void SpawnElement()
        {
            if (Camera.main == null)
            {
                return;
            }
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Planet.GetComponent<SphereCollider>().Raycast(ray, out var hitData, 1000))
            {
                var gameManager = GameManager.Instance(out var isNull);
                if (isNull)
                {
                    return;
                }
                // Spawns Herbivore
                if (Input.GetMouseButtonDown(1))
                {
                    _countHerbivore++;
                    var newHerb = ObjectPooler.Generate(Herbivore);
                    newHerb.transform.SetParent(Planet.transform, false);
                    newHerb.GetComponent<GravityBody>().Planet = Planet.GetComponent<GravityAttracter>();
                    newHerb.transform.position = hitData.point;
                    gameManager.UiManager.UpdateHerbivoreCount(_countHerbivore);
                }
                // Spawns Plant
                if (Input.GetMouseButtonDown(2))
                {
                    _countPlants++;
                    var newPlant = ObjectPooler.Generate(Plant);
                    newPlant.transform.SetParent(Planet.transform, false);
                    newPlant.GetComponent<GravityBody>().Planet = Planet.GetComponent<GravityAttracter>();
                    newPlant.transform.position = hitData.point;
                    gameManager.UiManager.UpdatePlantCount(_countPlants);
                }
            }
        }

        public void DestroyPlant(GameObject obj)
        {
            _countPlants--;
            ObjectPooler.Destroy(obj);
            var gameManager = GameManager.Instance(out var isNull);
            if (isNull)
            {
                return;
            }
            gameManager.UiManager.UpdatePlantCount(_countPlants);
        }
    }
}