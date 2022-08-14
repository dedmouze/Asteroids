using UnityEngine;
using UnityEngine.Audio;

public abstract class ConfigSO : ScriptableObject
{
    [Header("Sfx")]
    public AudioClip EffectSound;
    public AnimationClip EffectAnimation;
    public AudioMixerGroup AudioMixerGroup;
}