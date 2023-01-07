using TMPro;
using UnityEngine;

namespace Ui
{
    public class UiManager : MonoBehaviour
    {
        public TextMeshProUGUI herbivoreCount;
        public TextMeshProUGUI plantCount;

        public void UpdateHerbivoreCount(int count)
        {
            herbivoreCount.text = "Herbivore Count: " + count.ToString();
        }
    
        public void UpdatePlantCount(int count)
        {
            plantCount.text = "Plant Count: " + count.ToString();
        }
    }
}
