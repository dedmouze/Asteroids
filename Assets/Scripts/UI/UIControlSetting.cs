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

    private void Awake() => _controlButtonImage = _changeControlButton.GetComponent<Image>();
    
    private void OnEnable() => _changeControlButton.onClick.AddListener(ChangeControlType);
    private void OnDisable() => _changeControlButton.onClick.RemoveListener(ChangeControlType);
    
    private void ChangeControlType()
    {
        _controlType = (ControlType) ((int)(_controlType + 1) % _controlTypeLength);
        
        _controlButtonImage.sprite = _controlTypeSprites[(int)_controlType];
        
        EventBus.RaiseEvent<IControlTypeSubscriber>(s => s.OnControlTypeChanged(_controlType));
    }
}