using RayFire;
using UnityEngine;

public class NetController : MonoBehaviour
{
    [SerializeField] private Button _button;
    private RayfireGun _rayFireGun;

    private void Awake()
    {
        _rayFireGun = GetComponent<RayfireGun>();
    }
    private void Update()
    {
        if (_button.InButton) return;
        if (Input.GetMouseButtonDown(0))
        {
            _rayFireGun.Shoot();
        }
    }
}
