using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Sfx : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    private SfxFactory _sfxFactory;

    private Timer _lifeTimer;
    private float _maxTimeLength;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _lifeTimer = new Timer(float.MaxValue, () => _sfxFactory.Reclaim(this));
    }

    public void Init(ConfigSO config, Vector2 position, SfxFactory factory)
    {
        transform.position = position;
        _sfxFactory = factory;

        _audioSource.outputAudioMixerGroup = config.AudioMixerGroup;
        _audioSource.PlayOneShot(config.EffectSound);
        _animator.SetTrigger(config.EffectAnimation.name);
        
        _maxTimeLength = config.EffectSound.length > config.EffectAnimation.length ? config.EffectSound.length : config.EffectAnimation.length;
        _lifeTimer.SetNewTime(_maxTimeLength);
        Timers.Stop(_lifeTimer);
        Timers.Start(_lifeTimer);
    }
}