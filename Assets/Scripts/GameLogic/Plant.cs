using UnityEngine;

namespace GameLogic
{
    public class Plant : MonoBehaviour
    {
        private int _life = 5;

        public GameManager gameManager;

        public void DecreaseLife()
        {
            if (_life <= 0)
            {
                return;
            }
            _life--;
            if (_life <= 0)
            {
                gameManager.DeadPlant();
                Destroy(gameObject);
            }
        }
    }
}
