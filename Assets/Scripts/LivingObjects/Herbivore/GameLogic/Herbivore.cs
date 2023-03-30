
namespace LivingObjects.Herbivore.GameLogic
{
    public class Herbivore : LivingBody
    {
        protected override void Update()
        {
            base.Update();

            if (currentHungerLevel > LivingBodyAttributes.HungerLimitToDeath)
            {
                gameManager.ObjectSpawner.DestroyHerbivore(gameObject);
            }
        }
    }
}


