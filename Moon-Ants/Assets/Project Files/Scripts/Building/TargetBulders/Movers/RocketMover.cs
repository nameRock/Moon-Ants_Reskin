using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMover : MonoBehaviour
{
    [SerializeField] private float _positionY;
    [SerializeField] private float _durationFly;
    [SerializeField] private ParticleSystem _partFly;
    [SerializeField] private AudioSource _audioSource;

    public void Move()
    {
        if(_partFly != null)
        {
            _partFly.Play();
            _audioSource.Play();
        }

        transform.DOMove(new Vector3(transform.position.x, _positionY, transform.position.z), _durationFly).SetEase(Ease.Linear);
    }
}
