using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class Element : MonoBehaviour, IActivable
{
    [SerializeField] private Item _item;
    [SerializeField] private Collider _meshCollider;
    [SerializeField] private bool _isEnerge;
    [SerializeField] private float _scale;
    [SerializeField] private int _price;
    [SerializeField] private bool _isGreen;
    [SerializeField] private ElementAnimator _elementAnimator;
    [SerializeField] private ParticleSystem _otherParticls;

    private bool _isActive = true;
    private ElementSpawn _spawnPoint;

    public bool IsEnerge => _isEnerge;
    public bool IsGreen => _isGreen;
    public bool IsActive => _isActive;
    public float Scale => _scale;
    public int Price => _price;
    public Item Item => _item;
    public Element BaseElement { get; private set; }

    private void OnEnable()
    {
        if (_otherParticls != null)
        {
            _otherParticls.Play();
        }
    }

    private void OnDisable()
    {
        if (_otherParticls != null)
        {
            _otherParticls.Stop();
        }
    }

    public void Init(Element baseElement)
    {
        IncreaseSize(0);
        _price = baseElement.Price;
        BaseElement = baseElement;
    }

    public void DisableCollider()
    {
        _isActive = false;
        _meshCollider.enabled = false;

        if (_spawnPoint != null)
        {
            _spawnPoint.ChangedStatus(false);
        }
    }

    public void EnableCollider()
    {
        _meshCollider.enabled = true;
        IncreaseSize(0);
    }

    public void SetItem(Item item)
    {
        _item = item;
    }

    public void StopParticls()
    {
        if (_otherParticls != null)
        {
            _otherParticls.Stop();
        }
    }

    public void SetSpawnPoint(ElementSpawn elementSpawn)
    {

        _spawnPoint = elementSpawn;
    }

    public Element GetTemplateElement()
    {
        return _item.GiveAwayTemplate();
    }

    private void IncreaseSize(float duration)
    {
        transform.DOScale(new Vector3(0.3f, 0.3f, 2f), duration);
    }

    public void DisableElement()
    {
        _item.DisableElement();
        gameObject.SetActive(false);
    }

    public void PlayCollect(Transform collectPoint, Transform newParent, Action finishAction = null)
    {
        _elementAnimator.PlayCollectAnimation(collectPoint, newParent, finishAction);
    }

    public void PlayDrop(Transform target, Action OnAnimationComleted = null)
    {
        _elementAnimator.PlayDropAnimation(target, OnAnimationComleted);
    }
}
