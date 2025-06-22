using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Image _icon;
    [SerializeField] private CanvasGroup _canvasGroup;

    private Vector3 _targetScale = new Vector3(1.1f, 1.1f, 1.1f);

    public void Show(Reward reward)
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;
        _label.text = reward.RewardValue.ToString();
        _icon.sprite = reward.RewardIcon;
        transform.DOScale(_targetScale, 0.8f);
        _canvasGroup.DOFade(0, 0);
        Sequence fadeSequense = DOTween.Sequence();
        fadeSequense.Append(_canvasGroup.DOFade(1, 0.5f));
        fadeSequense.AppendInterval(0.3f);
        fadeSequense.Append(_canvasGroup.DOFade(0, 0.5f).OnComplete(() => gameObject.SetActive(false)));
    }
}