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
            if (_selectedCreature == CreatureType.Herbivore)
            {
                _selectedCreature = CreatureType.Empty;
                CurrentlySelected.text = "Currently Selected: None";
            }
            else
            {
                _selectedCreature = CreatureType.Herbivore;
                CurrentlySelected.text = "Currently Selected: Herbivore";
            }
        }

        public void SelectCarnivore()
        {
            if (_selectedCreature == CreatureType.Carnivore)
            {
                _selectedCreature = CreatureType.Empty;
                CurrentlySelected.text = "Currently Selected: None";
            }
            else
            {
                _selectedCreature = CreatureType.Carnivore;
                CurrentlySelected.text = "Currently Selected: Carnivore";
            }
        }


        public void SelectPlant()
        {
            if (_selectedCreature == CreatureType.Plant)
            {
                _selectedCreature = CreatureType.Empty;
                CurrentlySelected.text = "Currently Selected: None";
            }
            else
            {
                _selectedCreature = CreatureType.Plant;
                CurrentlySelected.text = "Currently Selected: Plant";
            }
        }
    }
}