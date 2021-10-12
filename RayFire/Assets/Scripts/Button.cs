using UnityEngine;
using UnityEngine.EventSystems;

public  class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool InButton { get; private set; }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        InButton = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        InButton = false;
    }
}
