using DG.Tweening;
using TMPro;
using UnityEngine;

public class AddBlockInfoToogler : MonoBehaviour
{
    [SerializeField] private WebSdk _webSdk;
    [SerializeField] private Canvas _adBlockCanvas;
    [SerializeField] private TMP_Text _infoText;

    private Tween _animationTween;

#if !CRAZY_GAMES
    private void OnEnable()
    {
        _webSdk.AdBlockDetected += OnAdBlockDetected;
    }

    private void OnDisable()
    {
        _webSdk.AdBlockDetected -= OnAdBlockDetected;
    }

    private void OnAdBlockDetected()
    {
        if(_animationTween != null && _animationTween.active == true)
        {
            _animationTween.Kill();
            _infoText.rectTransform.localScale = Vector3.one;
        }
        _adBlockCanvas.gameObject.SetActive(true);
        _animationTween = _infoText.rectTransform.DOScale(1.1f, 0.5f).SetEase(Ease.Linear).SetLoops(6, LoopType.Yoyo);
        _animationTween.OnComplete(() => _adBlockCanvas.gameObject.SetActive(false));
    }   
#endif
}
