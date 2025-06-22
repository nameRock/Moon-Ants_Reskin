using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class SizeChangedAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 _startSize;
    [SerializeField] private SizeAndDuration[] _sizesAndDurations;
    [SerializeField] private Ease _ease = Ease.Linear;
    [SerializeField] private bool _playInOnEnable;

    private Sequence _scalerSequence;

    private void OnEnable()
    {
        if (_playInOnEnable == true)
            Play();
    }
    public void Play(float delay = 0, bool isUnscaled = false)
    {
        transform.localScale = _startSize;
        _scalerSequence = DOTween.Sequence();
        if (delay > 0)
            _scalerSequence.SetDelay(delay);
        for (int i = 0; i < _sizesAndDurations.Length; i++)
        {
            _scalerSequence.Append(transform.DOScale(_sizesAndDurations[i].Size, _sizesAndDurations[i].Duration).SetEase(_ease));
        }
        _scalerSequence.SetUpdate(isUnscaled);
        
    }

    public void Hide(float duration = 0.5f)
    {
        transform.DOScale(0, duration);
      
        if(TryGetComponent(out Image image))
        {
            image.DOFade(0, duration);
        }
    }
}

[Serializable]
struct SizeAndDuration
{
    public Vector3 Size;
    public float Duration;
}
