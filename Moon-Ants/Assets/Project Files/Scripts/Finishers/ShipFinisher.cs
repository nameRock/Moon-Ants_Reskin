using UnityEngine;

public class ShipFinisher : Finisher
{
    [SerializeField] private CharacterMover _characterMover;
    [SerializeField] private ParticleSystem _effects;
    [SerializeField] private Transform _swimpingBody;
    [SerializeField] private bool _isSwimp = false;
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        if (_isSwimp == false)
            return;

        Vector3 rotatedPosition = new Vector3(3, 0, 0);
        float duration = 2f;
        _characterMover.LoopsRotate(_swimpingBody, rotatedPosition, duration);   
    }

    protected override void Launch()
    {
        _effects.Play();
        _audioSource.Play();
        _characterMover.MoveToPatch();
    }    
}
