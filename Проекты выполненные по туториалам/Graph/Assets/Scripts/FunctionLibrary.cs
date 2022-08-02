using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t);
    private static readonly Function[] _functions = {Wave, MultiWave, Ripple, Sphere, TwistedSphere, Torus, TwistedTorus};
    public enum FunctionName {Wave, MultiWave, Ripple, Sphere, TwistedSphere, Torus, TwistedTorus}
    
    public static Function GetFunction(FunctionName name)
    {
        return _functions[(int)name];
    }
    public static FunctionName GetNextFunctionName(FunctionName name)
    {
        return (int) name < _functions.Length - 1 ? name + 1 : 0;
    }
    public static FunctionName GetNextRandomFunctionName(FunctionName name)
    {
        var choice = (FunctionName)Random.Range(0, _functions.Length);
        return choice == name ? 0 : choice;
    }
    public static Vector3 Morph(float u, float v, float t, Function from, Function to, float progress)
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }
    private static Vector3 Wave(float u, float v, float t)
    {
        Vector3 func;
        func.x = u;
        func.y = Sin(PI * (u + v + t));
        func.z = v;
        return func;
    }
    private static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 func;
        func.x = u;
        func.y = Sin(PI * (u + 0.5f * t));
        func.y += 0.5f * Sin(2f * PI * (v + t));
        func.y += Sin(PI * (u + v + 0.25f * t));
        func.y *= 1f / 2.5f;
        func.z = v;
        return func;
    }
    private static Vector3 Ripple(float u, float v, float t)
    {
        Vector3 func;
        float hyp = Sqrt(u * u + v * v);
        func.x = u;
        func.y = Sin(PI * (4 * hyp - t));
        func.y /= 1f + 10f * hyp;
        func.z = v;
        return func;
    }
    private static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 func;
        float rad = 1f;
        float s = rad * Cos(0.5f * PI * v);
        func.x = s * Sin(PI * u);
        func.y = rad * Sin(PI * 0.5f * v);
        func.z = s * Cos(PI * u);
        return func;
    }
    private static Vector3 TwistedSphere(float u, float v, float t)
    {
        Vector3 func;
        float rad = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = rad * Cos(0.5f * PI * v);
        func.x = s * Sin(PI * u);
        func.y = rad * Sin(PI * 0.5f * v);
        func.z = s * Cos(PI * u);
        return func;
    }
    private static Vector3 Torus(float u, float v, float t)
    {
        Vector3 func;
        float rad1 = 0.75f;
        float rad2 = 0.25f;
        float s = rad1 + rad2 * Cos(PI * v);
        func.x = s * Sin(PI * u);
        func.y = rad2 * Sin(PI * v);
        func.z = s * Cos(PI * u);
        return func;
    }
    private static Vector3 TwistedTorus(float u, float v, float t)
    {
        Vector3 func;
        float rad1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float rad2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = rad1 + rad2 * Cos(PI * v);
        func.x = s * Sin(PI * u);
        func.y = rad2 * Sin(PI * v);
        func.z = s * Cos(PI * u);
        return func;
    }
}
