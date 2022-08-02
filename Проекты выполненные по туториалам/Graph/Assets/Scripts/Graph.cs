using UnityEngine;
public class Graph : MonoBehaviour
{
    [SerializeField] private Transform _pointPrefab;
    [SerializeField, Range(10, 100)] private int _resolution = 10;
    [SerializeField] private FunctionLibrary.FunctionName _function;
    [SerializeField, Min(0f)] private float _functionDuration = 1f, _transitionDuration = 1f;
    [SerializeField] private TransitionMode _transitionMode;
    private enum TransitionMode {Cycle, Random}
    private float _duration;
    private Transform[] _points;
    private bool _transitioning;
    private FunctionLibrary.FunctionName _transitionFunction;
    private void Awake()
    {
        _points = new Transform[_resolution * _resolution];
        float step = 2f / _resolution;
        var scale = Vector3.one * step;
        for (int i = 0, n = _points.Length; i < n; i++)
        {
            Transform point = _points[i] = Instantiate(_pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
        }
    }
    private void Update()
    {
        _duration += Time.deltaTime;
        if (_transitioning)
        {
            if (_duration >= _transitionDuration)
            {
                _duration -= _transitionDuration;
                _transitioning = false;
            }
        }
        else if (_duration >= _functionDuration)
        {
            _duration -= _functionDuration;
            _transitionFunction = _function;
            _transitioning = true;
            PickNextFunction();
        }
        if(_transitioning)
            UpdateFunctionTransition();
        else
            UpdateFunction();
    }
    private void PickNextFunction()
    {
        if(_transitionMode == TransitionMode.Cycle)
            _function = FunctionLibrary.GetNextFunctionName(_function);
        if (_transitionMode == TransitionMode.Random)
            _function = FunctionLibrary.GetNextRandomFunctionName(_function);
    }
    private void UpdateFunction()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(_function);
        float time = Time.time;
        float step = 2f / _resolution;
        float v = 0.5f * step - 1;
        for (int i = 0, x = 0, z = 0, n = _points.Length; i < n; i++, x++)
        {
            if (x == _resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            _points[i].localPosition = f(u, v, time);
        }
    }
    private void UpdateFunctionTransition()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(_function);
        FunctionLibrary.Function 
            from = FunctionLibrary.GetFunction(_transitionFunction),
            to = FunctionLibrary.GetFunction(_function);
        float progress = _duration / _transitionDuration;
        float time = Time.time;
        float step = 2f / _resolution;
        float v = 0.5f * step - 1;
        for (int i = 0, x = 0, z = 0, n = _points.Length; i < n; i++, x++)
        {
            if (x == _resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            _points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
        }
    }
}
