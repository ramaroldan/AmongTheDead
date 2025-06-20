using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool isOverElement;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOverElement = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOverElement= false;
    }
    
    public bool IsOverElement()
    {
        return isOverElement;
    }
}
