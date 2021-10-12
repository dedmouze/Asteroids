using UnityEngine;

public class FreeModeButton : Button
{
    [SerializeField] private CameraRotation _camera;
    
    public void SwitchMode()
    {
        _camera.CameraMode = _camera.CameraMode == CameraMode.None ? CameraMode.Free : CameraMode.None;
    }
}
