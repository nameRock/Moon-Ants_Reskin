using UnityEngine;

public class PlaneFinisher : Finisher
{
    [SerializeField] private CharacterMover _characterMover;
    [SerializeField] private ParticleSystem _effects;
    [SerializeField] private GameObject _deactivationBoby;
    [SerializeField] private GameObject _activationBody;
    [SerializeField] private AudioSource _audioSource;
   
    protected override void Launch()
    {
        _audioSource.Play();
        _deactivationBoby.SetActive(false);
        _activationBody.SetActive(true);
        _effects.Play();
        _characterMover.MoveToPatch(DG.Tweening.Ease.InCubic);
    }   
}
