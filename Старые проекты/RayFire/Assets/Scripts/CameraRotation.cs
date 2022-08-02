using UnityEngine;

public enum CameraMode
{
    None,
    Free
}
public class CameraRotation : MonoBehaviour
{
    private Vector3 _offset;
    public CameraMode CameraMode { get; set; }
    
    private void Start()
    {
        CameraMode = CameraMode.None;
        _offset = transform.position;
    }
    private void Update()
    {
        if (CameraMode == CameraMode.Free)
        {
            if (Input.GetMouseButton(1))
            {
                Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 5f, Vector3.up);
                _offset = camTurnAngle * _offset;
            }
        }
        transform.position = _offset;
        transform.LookAt(Vector3.zero);
    }
}
