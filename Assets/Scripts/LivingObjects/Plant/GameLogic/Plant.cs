using _Core;
using UnityEngine;

namespace LivingObjects.Plant.GameLogic
{
    public class Plant : MonoBehaviour
    {
        private int _life = 5;

        public void DecreaseLife()
        {
            if (_life <= 0)
            {
                return;
            }
            _life--;
            if (_life <= 0)
            {
                var gameManager = GameManager.Instance(out var isNull);
                if (isNull)
                {
                    return;
                }
                gameManager.ObjectSpawner.DestroyPlant(gameObject);
            }
        }
    }
}
