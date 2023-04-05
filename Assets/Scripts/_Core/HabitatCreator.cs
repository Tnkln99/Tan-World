using _Core;
using Models.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HabitatCreator : MonoBehaviour, IDragHandler, IPointerClickHandler
{
    public bool isCreatingHabitat = false;

    public void OnDrag(PointerEventData eventData)
    {
        if (!isCreatingHabitat) { return; }
        Debug.Log("On Drag");
        //throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isCreatingHabitat) { return; }
        Debug.Log("On Pointer Click");
        //throw new System.NotImplementedException();
    }

    public void SelectHabitatCreation()
    {
        var gameManager = GameManager.Instance(out var isNull);
        if (isNull)
        {
            return;
        }

        if (isCreatingHabitat)
        {
            gameManager.UiManager.UpdateSelectedTypeText("Empty");
        }
        else
        {
            gameManager.UiManager.UpdateSelectedTypeText("Habitat Selection");
        }
        isCreatingHabitat = !isCreatingHabitat;
    }
}
