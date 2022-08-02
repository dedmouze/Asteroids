using UnityEngine;

[System.Serializable]
public struct ColorRangeHSV
{
    [FloatRangeSlider(0f, 1f)]
    public FloatRange Hue, Saturation, Value;
    public Color RandomInRange => Random.ColorHSV(
        Hue.Min, Hue.Max,
        Saturation.Min, Saturation.Max,
        Value.Min, Value.Max,
        1f, 1f);
}
