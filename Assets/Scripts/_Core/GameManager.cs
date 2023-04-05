using Ui;
using UnityEngine;

namespace _Core
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public Camera MainCamera;
        public UiManager UiManager;
        public ObjectSpawner ObjectSpawner;
        public HabitatCreator HabitatCreator;

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
    }
}