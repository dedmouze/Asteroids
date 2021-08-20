using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeInput : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private Vector3 _direction;
    private PlayerMovement _playerMovement;

    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
    }
    //Swipe
    public void OnBeginDrag(PointerEventData eventData)
    {
        if((Mathf.Abs(eventData.delta.x)) > (Mathf.Abs(eventData.delta.y)))
        {
            if(eventData.delta.x > 0)
            {
                _direction = Vector3.right;
            }
            else
            {
                _direction = Vector3.left;
            }
        }
        else
        {
            if(eventData.delta.y > 0)
            {
                _direction = Vector3.forward;
            }
            else
            {
                _direction = Vector3.back;
            }
        }

        _playerMovement.SetDirection(_direction);

    }
    public void OnDrag(PointerEventData eventData){}
    //End swipe
}
