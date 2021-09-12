using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform _hoursPivot, _minutesPivot, _secondsPivot;
    const float _hoursToDegrees = -30f;
    const float _minutesToDegrees = -6f;
    const float _secondsToDegrees = -6f;

    private void Update()
    {
        var time = DateTime.Now.TimeOfDay;
        _hoursPivot.localRotation = Quaternion.Euler(0f, 0f, _hoursToDegrees * (float)time.TotalHours);
        _minutesPivot.localRotation = Quaternion.Euler(0f, 0f, _minutesToDegrees * (float)time.TotalMinutes);
        _secondsPivot.localRotation = Quaternion.Euler(0f, 0f, _secondsToDegrees * (float)time.TotalSeconds);
    }
}
