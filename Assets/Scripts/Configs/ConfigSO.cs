using UnityEngine;
using UnityEngine.Audio;

public abstract class ConfigSO : ScriptableObject
{
    [Header("Sfx")]
    [SerializeField] private AudioClip _effectSound;
    [SerializeField] private AnimationClip _effectAnimation;
    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    
    public AudioClip EffectSound => _effectSound;
    public AnimationClip EffectAnimation => _effectAnimation;
    public AudioMixerGroup AudioMixerGroup => _audioMixerGroup;
}