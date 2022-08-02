using UnityEngine;

public class ScaleTransformation : Transformation
{
    [SerializeField] private Vector3 _scale = Vector3.one;

    public override Matrix4x4 Matrix 
    {
        get
        {
            Matrix4x4 matrix = new Matrix4x4();
            matrix.SetRow(0, new Vector4(_scale.x, 0f, 0f, 0f));
            matrix.SetRow(1, new Vector4(0f, _scale.y, 0f, 0f));
            matrix.SetRow(2, new Vector4(0f, 0f, _scale.z, 0f));
            matrix.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
            return matrix;
        } 
    }
}
