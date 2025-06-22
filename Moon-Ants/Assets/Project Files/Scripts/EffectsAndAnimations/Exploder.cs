using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _explosionPower;
    [SerializeField] private float _radius = 30;
    [SerializeField] private Transform _parts;
    [SerializeField] private Renderer[] _meshRenderers;
    [SerializeField] private ParticleSystem _explodeEffect;
    [SerializeField] private float _deadDelay = 5f;

    private bool _isExploded = false;
    private Rigidbody[] _rigidbodies;

    private void Awake()
    {
        _rigidbodies = _parts.GetComponentsInChildren<Rigidbody>();
        _parts.gameObject.SetActive(false);
    }

    private void RemoveParts()
    {
        _isExploded = true;
        _parts.SetParent(null);
        _parts.gameObject.SetActive(true);
        Destroy(_parts.gameObject, _deadDelay);
        foreach (var mesh in _meshRenderers)
        {
            mesh.enabled = false;
        }
    }

    private void Explode(Vector3 position, float force, float radius)
    {
        if (_isExploded)
            return;

        RemoveParts();

        foreach (var rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.AddExplosionForce(force, position, radius);
        }

        if (_explodeEffect != null)
            Instantiate(_explodeEffect, transform.position + Vector3.up * 0.8f, Quaternion.identity);
        gameObject.SetActive(false);
    }


    public void Activate(Vector3 position)
    {
        Explode(position, _explosionPower, _radius);
    }    
}
