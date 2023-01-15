using Models.Enums;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class SelectionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI CurrentlySelected;

        private CreatureType _selectedCreature = CreatureType.Empty;

        public void SelectHerbivore()
        {
            _selectedCreature = CreatureType.Herbivore;
            CurrentlySelected.text = "Selected: Herbivore";
        }

        public void SelectCarnivore()
        {
            _selectedCreature = CreatureType.Carnivore;
            CurrentlySelected.text = "Selected: Carnivore";
        }

        public void SelectEmpty()
        {
            _selectedCreature = CreatureType.Empty;
            CurrentlySelected.text = "Selected: None";
        }

        public void SelectPlant()
        {
            _selectedCreature = CreatureType.Plant;
            CurrentlySelected.text = "Selected: Plant";
        }
    }
}