using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryUI parent;

    public void OnPointerEnter(PointerEventData eventData)
    {
        int index = 0;
        for(int i = 0; i < transform.parent.childCount; i++)
        {
            if(transform.parent.GetChild(i) == transform)
            {
                index = i;
            }
        }
        parent.descriptionBox.text = parent.controller.inventory.GetAllItems()[index].description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        parent.descriptionBox.text = "";
    }
}
