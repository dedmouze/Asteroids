using UnityEngine;
using UnityEngine.UI;

public class UILife : MonoBehaviour
{
    [SerializeField] private Image[] _lifes;

    private ShipMovement _ship;
    
    private void Awake() => _ship = FindObjectOfType<ShipMovement>();

    private void OnEnable() => _ship.ShipBlown += DecreaseLife;
    private void OnDisable() => _ship.ShipBlown -= DecreaseLife;
    
    private void DecreaseLife()
    {
        for (int i = _lifes.Length - 1; i >= 0; i--)
        {
            if (_lifes[i].gameObject.activeSelf)
            {
                _lifes[i].gameObject.SetActive(false);
                return;
            }
        }
    }
}