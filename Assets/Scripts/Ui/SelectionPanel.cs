using Models.Enums;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class SelectionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI CurrentlySelected;

        public void UpdateSelection(CreatureType type)
        {
            CurrentlySelected.text = type.ToString();
        }
    }
}