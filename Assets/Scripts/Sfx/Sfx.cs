using UnityEngine;

public class Sfx : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    private SfxFactory _sfxFactory;

    private Timer _timer;
    private float _maxTimeLength;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _timer = new Timer(1000f, Reclaim);
    }

    public void Init(ConfigSO config, Vector2 position, SfxFactory factory)
    {
        transform.position = position;
        _sfxFactory = factory;

        _audioSource.outputAudioMixerGroup = config.AudioMixerGroup;
        _audioSource.PlayOneShot(config.EffectSound);
        _animator.SetTrigger(config.EffectAnimation.name);
        
        _maxTimeLength = config.EffectSound.length > config.EffectAnimation.length ? config.EffectSound.length : config.EffectAnimation.length;
        _timer.SetNewTime(_maxTimeLength);
    }

    private void Update() => _timer.Tick(Time.deltaTime);
    
    private void Reclaim() => _sfxFactory.Reclaim(this);
}