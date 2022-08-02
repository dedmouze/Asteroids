using System;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Explosion : WarEntity {

    [SerializeField, Range(0f, 1f)] private float _duration = 0.5f;
    private float _age;
    
    [SerializeField] private AnimationCurve _opacityCurve;
    [SerializeField] private AnimationCurve _scaleCurve;
    private static readonly int colorPropertyID = Shader.PropertyToID("_Color");
    private static MaterialPropertyBlock _propertyBlock;
    private MeshRenderer _meshRenderer;
    private float _scale;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        Debug.Assert(_meshRenderer != null, "Explosion without renderer!");
    }
    
    public void Initialize (Vector3 position, float blastRadius, float damage = 0) 
    {
        if(damage > 0)
        {
            TargetPoint.FillBuffer(position, blastRadius);
            for (int i = 0; i < TargetPoint.BufferedCount; i++)
            {
                TargetPoint.GetBuffered(i).Enemy.ApplyDamage(damage);
            }
        }
        transform.localPosition = position;
        _scale = 2f * blastRadius;
    }
    public override bool GameUpdate () 
    {
        _age += Time.deltaTime;
        if (_age >= _duration) 
        {
            OriginFactory.Reclaim(this);
            return false;
        }

        _propertyBlock ??= new MaterialPropertyBlock();
        float time = _age / _duration;
        Color color = Color.clear;
        color.a = _opacityCurve.Evaluate(time);
        _propertyBlock.SetColor(colorPropertyID, color);
        _meshRenderer.SetPropertyBlock(_propertyBlock);
        transform.localScale = Vector3.one * (_scale * _scaleCurve.Evaluate(time));
        return true;
    }
}