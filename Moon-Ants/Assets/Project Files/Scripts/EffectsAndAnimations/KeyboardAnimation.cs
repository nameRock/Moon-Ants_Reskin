using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KeyboardAnimation : MonoBehaviour
{
    [SerializeField] private Image[] _keysBackgrounds;
    [SerializeField] private float _targetAlpha = 0.75f;
    [SerializeField] private float _duration = 0.3f;
         
    Tween _keysTween;

    private void Start()
    {
        Sequence keysSequence= DOTween.Sequence();
        foreach (var key in _keysBackgrounds)
        {
            keysSequence.Append(key.DOFade(_targetAlpha, _duration).SetLoops(4, LoopType.Yoyo).SetEase(Ease.Linear));
        }
        keysSequence.SetLoops(-1);
        _keysTween = keysSequence;
    }

    private void OnDisable()
    {
        if (_keysTween != null)
            _keysTween.Kill();
    }
}
