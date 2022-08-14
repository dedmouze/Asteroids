using System;
using UnityEngine;
using UnityEngine.UI;

public class UIControlSetting : MonoBehaviour
{
    [SerializeField] private Button _changeControlButton;
    [SerializeField] private Sprite[] _controlTypeSprites;

    private readonly int _controlTypeLength = Enum.GetNames(typeof(ControlType)).Length;
    private ControlType _controlType;
    
    private Image _controlButtonImage;
    
    public event Action<ControlType> ControlTypeChanged;

    private void Start()
    {
        _controlButtonImage = _changeControlButton.GetComponent<Image>();
        
        _controlType = Game.Instance.ControlType;
        _controlButtonImage.sprite = _controlTypeSprites[(int) _controlType];
    }
    
    private void OnEnable() => _changeControlButton.onClick.AddListener(ChangeControlType);
    private void OnDisable() => _changeControlButton.onClick.RemoveListener(ChangeControlType);
    
    private void ChangeControlType()
    {
        _controlType = (ControlType) ((int)(_controlType + 1) % _controlTypeLength);
        
        _controlButtonImage.sprite = _controlTypeSprites[(int)_controlType];
        
        ControlTypeChanged?.Invoke(_controlType);
    }
}