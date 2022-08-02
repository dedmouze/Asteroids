using UnityEngine;

[System.Serializable]
public struct FloatRange
{
    public float Min, Max;
    public float RandomValueInRange => Random.Range(Min, Max);
}
