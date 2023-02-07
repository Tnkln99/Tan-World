using System;
using System.Collections.Generic;
using Gravity;
using Helpers.ObjectPool;
using Models.Enums;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;


namespace _Core
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private ObjectPooler ObjectPooler;
        [SerializeField] private GameObject Herbivore;
        [SerializeField] private GameObject Carnivore;
        [SerializeField] private GameObject Planet;
        [SerializeField] private GameObject Plant;
        private int _countHerbivore;
        private int _countPlants;
        private int _countCarnivores;


        private CreatureType _selectedCreature = CreatureType.Empty;

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
            pooledTypes.Add(new PooledType()
            {
                AutoExpand = true,
                Max = 1000,
                Prefab = Carnivore,
                SortingTag = "Carnivore",
                TypeName = "Carnivore"
            });
            ObjectPooler.Initialize(pooledTypes);
        }

        public void SpawnElement()
        {
            var gameManager = GameManager.Instance(out var isNull);
            if (isNull || gameManager.MainCamera == null)
            {
                return;
            }
            
            var ray = gameManager.MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Planet.GetComponent<SphereCollider>().Raycast(ray, out var hitData, 1000))
            {
                // Spawns Herbivore
                if (_selectedCreature == CreatureType.Herbivore)
                {
                    _countHerbivore++;
                    var newHerb = ObjectPooler.Generate(Herbivore);
                    newHerb.transform.SetParent(Planet.transform, false);
                    newHerb.GetComponent<GravityBody>().Planet = Planet.GetComponent<GravityAttracter>();
                    newHerb.transform.position = hitData.point;
                    gameManager.UiManager.UpdateHerbivoreCount(_countHerbivore);
                }

                // Spawns Carnivore
                if (_selectedCreature == CreatureType.Carnivore)
                {
                    _countCarnivores++;
                    var newCarn = ObjectPooler.Generate(Carnivore);
                    newCarn.transform.SetParent(Planet.transform, false);
                    newCarn.GetComponent<GravityBody>().Planet = Planet.GetComponent<GravityAttracter>();
                    newCarn.transform.position = hitData.point;
                    gameManager.UiManager.UpdateCarnivoreCount(_countCarnivores);
                }

                // Spawns Plant
                if (_selectedCreature == CreatureType.Plant)
                {
                    _countPlants++;
                    var newPlant = ObjectPooler.Generate(Plant);
                    newPlant.transform.SetParent(Planet.transform, false);
                    newPlant.GetComponent<GravityBody>().Planet = Planet.GetComponent<GravityAttracter>();
                    newPlant.transform.position = hitData.point;

                    var body = newPlant.transform;
                    var gravityUp = (body.position - transform.position).normalized;
                    var bodyUp = body.up;
                    
                    var targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * body.rotation;
                    
                    body.rotation = targetRotation;
                    
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

        public void DestroyHerbivore(GameObject obj)
        {
            _countHerbivore--;
            ObjectPooler.Destroy(obj);
            var gameManager = GameManager.Instance(out var isNull);
            if (isNull)
            {
                return;
            }

            gameManager.UiManager.UpdateHerbivoreCount(_countHerbivore);
        }

        public void DestroyCarnivore(GameObject obj)
        {
            _countCarnivores--;
            ObjectPooler.Destroy(obj);
            var gameManager = GameManager.Instance(out var isNull);
            if (isNull)
            {
                return;
            }

            gameManager.UiManager.UpdateCarnivoreCount(_countCarnivores);
        }

        public void SelectHerbivore()
        {
            var gameManager = GameManager.Instance(out var isNull);
            if (isNull)
            {
                return;
            }

            if (_selectedCreature == CreatureType.Herbivore)
            {
                _selectedCreature = CreatureType.Empty;
                gameManager.UiManager.UpdateSelectedTypeText(CreatureType.Empty);
            }
            else
            {
                _selectedCreature = CreatureType.Herbivore;
                gameManager.UiManager.UpdateSelectedTypeText(CreatureType.Herbivore);
            }
        }

        public void SelectCarnivore()
        {
            var gameManager = GameManager.Instance(out var isNull);
            if (isNull)
            {
                return;
            }

            if (_selectedCreature == CreatureType.Carnivore)
            {
                _selectedCreature = CreatureType.Empty;
                gameManager.UiManager.UpdateSelectedTypeText(CreatureType.Empty);
            }
            else
            {
                _selectedCreature = CreatureType.Carnivore;
                gameManager.UiManager.UpdateSelectedTypeText(CreatureType.Carnivore);
            }
        }


        public void SelectPlant()
        {
            var gameManager = GameManager.Instance(out var isNull);
            if (isNull)
            {
                return;
            }

            if (_selectedCreature == CreatureType.Plant)
            {
                _selectedCreature = CreatureType.Empty;
                gameManager.UiManager.UpdateSelectedTypeText(CreatureType.Empty);
            }
            else
            {
                _selectedCreature = CreatureType.Plant;
                gameManager.UiManager.UpdateSelectedTypeText(CreatureType.Plant);
            }
        }
    }
}