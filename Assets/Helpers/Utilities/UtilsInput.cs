using UnityEngine;

namespace Helpers.Utilities
{
    public static class UtilsInput
    {
        public static bool IsInputActive(this MonoBehaviour monoBehaviour)
        {
            var helperManager = HelperManager.Instance(out var isNull);
            if (isNull)
            {
                return false;
            }
            return monoBehaviour.gameObject.activeInHierarchy && helperManager.IsInputActive();
        }
    }
}