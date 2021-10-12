using RayFire;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Button _button;
    private RayfireGun _gun;
    private Plane _plane;
        
    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);

    private void Awake()
    {
        _gun = GetComponent<RayfireGun>();
    }
    private void Start()
    {
        _plane = new Plane(Vector3.forward, 0f);
    }
    private void Update()
    {
        if (_button.InButton) return;
        if (_plane.Raycast(TouchRay, out var distanceToPlane))
        {
            var hitPoint = TouchRay.GetPoint(distanceToPlane);
            transform.LookAt(hitPoint);
        }
        if (Input.GetMouseButtonDown(0))
        {
            _gun.Shoot();
        }
    }
}
