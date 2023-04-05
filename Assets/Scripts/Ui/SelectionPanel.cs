using Models.Enums;
using TMPro;
using UnityEngine;

namespace Ui
{
    public class SelectionPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI CurrentlySelected;

        public void UpdateSelection(string type)
        {
            CurrentlySelected.text = "Selected: " + type;
        }
    }
}