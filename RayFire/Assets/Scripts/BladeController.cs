using UnityEngine;

public class BladeController : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private float _speed;
    
    private Plane _plane;
    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    
    private void Update()
    {
        if (_button.InButton) return;
        _plane = new Plane(TouchRay.direction, 0f);
        if (_plane.Raycast(TouchRay, out var distanceToPlane))
        {
            var hitPoint = TouchRay.GetPoint(distanceToPlane);
            transform.position = Vector3.MoveTowards(transform.position, hitPoint, Time.deltaTime * _speed);
            transform.LookAt(hitPoint);
        }
    }
}
