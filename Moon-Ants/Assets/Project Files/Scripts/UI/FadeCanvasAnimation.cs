using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeCanvasAnimation : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField] private float _startAlpha = 0;

    private CanvasGroup _canvasGroup;
    private Tweener _fadeTweener;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = _startAlpha;
    }

    public void ChangeAlpha(float targetAlpha)
    {
        if (_fadeTweener != null && _fadeTweener.active == true)
        {
            float lastAlpha = _canvasGroup.alpha;
            _fadeTweener.Complete();
            _canvasGroup.alpha = lastAlpha;
        }
        _fadeTweener = _canvasGroup.DOFade(targetAlpha, _fadeDuration);
    }
}
