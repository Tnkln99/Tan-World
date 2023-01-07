using UnityEngine;

namespace GameLogic
{
    public class AiHerbivore : AiBehavior
    {
        protected override void Eating(ref Vector3 direction)
        {
            base.Eating(ref direction);
            direction = new Vector3(0, 0,  0);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 3)
            {
                return;
            }

            if (other.CompareTag("Plant"))
            {
                state = State.Eating;
                other.gameObject.GetComponent<Plant>().DecreaseLife();
            }
        }
    }
}
