using UnityEngine;

namespace Models.ScriptableObjectModels
{
    [CreateAssetMenu(fileName = "LivingBodyAttributes", menuName = "Scriptable/LivingBodyAttributes/LivingBodyAttributes")]
    public class LivingBodyAttributes : ScriptableObject
    {
        public float Speed;
        public float DetectionRad;
        public float HungerLimitToLookForFood;
        public float HungerLimitToDeath;
    }
}