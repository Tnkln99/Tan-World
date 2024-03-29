using Models.Enums;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI HerbivoreCount;
        [SerializeField] private TextMeshProUGUI CarnivoreCount;
        [SerializeField] private TextMeshProUGUI PlantCount;
        [SerializeField] private SelectionPanel selectionPanel;

        public void UpdateHerbivoreCount(int count)
        {
            HerbivoreCount.text = "Herbivore Count: " + count;
        }

        public void UpdateCarnivoreCount(int count)
        {
            CarnivoreCount.text = "Carnivore Count: " + count;
        }

        public void UpdatePlantCount(int count)
        {
            PlantCount.text = "Plant Count: " + count;
        }

        public void UpdateSelectedTypeText(CreatureType type)
        {
            selectionPanel.UpdateSelection(type);
        }
    }
}