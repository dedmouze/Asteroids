using UnityEngine;
using UnityEngine.UI;

public class UILife : MonoBehaviour
{
    [SerializeField] private Image[] _lifes;

    private Ship _ship;
    
    private void Awake() => _ship = FindObjectOfType<Ship>();

    private void OnEnable() => _ship.LifeDecreased += DecreaseLife;
    private void OnDisable() => _ship.LifeDecreased -= DecreaseLife;
    
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
