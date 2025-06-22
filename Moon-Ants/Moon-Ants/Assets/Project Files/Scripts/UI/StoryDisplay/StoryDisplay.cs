using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Krivodeling.UI.Effects;
using DG.Tweening;
using System;

public class StoryDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _display;
    [SerializeField] private TextAnimator _textAnimator;
    [SerializeField] private SizeChangedAnimation _cloudSizeAnimation;
    [SerializeField] private Image _antImage;
    [SerializeField] private UIBlur _uIBlur;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _finishButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private float _nextTextDelay = 5f;
    [SerializeField] private float _finichDelay = 5f;
    [SerializeField] private LocalizationID _localizationID;
    [SerializeField] private CanvasGroup[] _hiddenObjects;
    [SerializeField] private Image _backgroundImage;

    private Coroutine _waitAnimatedText;
    private Coroutine _waitShowStory;
    private Coroutine _waitClosedDisplay;
    private float _backgroundAlpha;
    public static event Action<bool> StoryPlayed;

    private void Awake()
    {
        DeactivateButtons();
        _backgroundAlpha = _backgroundImage.color.a;
    }

    private IEnumerator Start()
    {
        foreach (var canvas in _hiddenObjects)
        {
            canvas.alpha = 0;
        }
        _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 0);
        yield return null;
        float startDelay = 1.5f;
        float backGroundTime = 0.7f;
        _uIBlur.Init();
        _uIBlur.BeginBlur(backGroundTime);
        _backgroundImage.gameObject.SetActive(true);
        _backgroundImage.DOFade(_backgroundAlpha, backGroundTime).SetUpdate(true);

        yield return new WaitForSeconds(startDelay);
        _display.SetActive(true);
        StartShowText();
    }

    private void OnEnable()
    {       
        _nextButton.onClick.AddListener(OnNextButtonClick);
        _finishButton.onClick.AddListener(OnFinishButtonClick);
        _closeButton.onClick.AddListener(OnCloseButtonClicK);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(OnNextButtonClick);
        _finishButton.onClick.RemoveListener(OnFinishButtonClick);
        _closeButton.onClick.RemoveListener(OnCloseButtonClicK);
    }

    private void OnNextButtonClick()
    {
        if (_waitShowStory != null)
            StopCoroutine(_waitShowStory);
        _textAnimator.IncreaseTextIndex();
        _nextButton.gameObject.SetActive(false);
        _waitShowStory = StartCoroutine(WaitShowStory(0));
    }

    private void OnFinishButtonClick()
    {
        if (_waitClosedDisplay == null)
            _waitClosedDisplay = StartCoroutine(WaitClosedDisplay());
    }

    private void OnCloseButtonClicK()
    {
        _nextButton.gameObject.SetActive(false);

        if (_waitClosedDisplay == null)
            _waitClosedDisplay = StartCoroutine(WaitClosedDisplay());
        _waitClosedDisplay = StartCoroutine(WaitClosedDisplay());
    }

    private void StartShowText()
    {
        string localization = Localization.TranslationData.GetTranslation(_localizationID);
        _textAnimator.Init(localization, Localization.TranslationData.Font);
        _cloudSizeAnimation.transform.localScale = Vector3.zero;
        _cloudSizeAnimation.Play(1f, true);
        ShowAnt();
        _waitShowStory = StartCoroutine(WaitShowStory(1.5f));
        StoryPlayed?.Invoke(true);
    }

    private IEnumerator WaitShowStory(float startDelay)
    {
        yield return new WaitForSecondsRealtime(startDelay);
        while (_textAnimator.TextIndex < _textAnimator.TextBlocksLenght)
        {
            _waitAnimatedText = StartCoroutine(_textAnimator.WaitTextAnimation());
            yield return _waitAnimatedText;

            if (_textAnimator.TextIndex + 1 < _textAnimator.TextBlocksLenght)
            {
                _nextButton.gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(_nextTextDelay);
                _nextButton.gameObject.SetActive(false);
            }
            _textAnimator.IncreaseTextIndex();
        }

        _finishButton.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(_finichDelay);
        _waitClosedDisplay = StartCoroutine(WaitClosedDisplay());
    }

    private IEnumerator WaitClosedDisplay()
    {
        if (_waitShowStory != null)
            StopCoroutine(_waitShowStory);
        StoryPlayed?.Invoke(false);
        _finishButton.gameObject.SetActive(false);
        float hideDelay = 0.5f;
        _cloudSizeAnimation.Hide();

        float backGroundTime = 1f;
        _uIBlur.EndBlur(backGroundTime);       
        _backgroundImage.DOFade(0, backGroundTime).OnComplete(() => _backgroundImage.gameObject.SetActive(false));

        yield return new WaitForSecondsRealtime(hideDelay);
        float closedDelay = 1f;

        _antImage.transform.DOMove(new Vector3(-200, -200), 2);
        yield return new WaitForSecondsRealtime(closedDelay);
        foreach (var canvas in _hiddenObjects)
        {
            canvas.DOFade(1, 0.5f);
        }
        _display.SetActive(false);
        _uIBlur.gameObject.SetActive(false);
    }

    private void ShowAnt()
    {
        Vector3 startPosition = _antImage.transform.position;
        _antImage.transform.position = new Vector3(-200, -200);
        _antImage.transform.DOMove(startPosition, 1f).SetUpdate(true);
    }

    private void DeactivateButtons()
    {
        _finishButton.gameObject.SetActive(false);
        _nextButton.gameObject.SetActive(false);
    }
}
