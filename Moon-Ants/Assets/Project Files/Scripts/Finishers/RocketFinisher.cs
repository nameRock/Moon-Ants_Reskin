using UnityEngine;

public class RocketFinisher : Finisher
{
    [SerializeField] private RocketMover _rocketMover;
    [SerializeField] private AudioSource _audioSource;

    protected override void Launch()
    {
        _audioSource.Play();
        _rocketMover.Move();
    }
}
