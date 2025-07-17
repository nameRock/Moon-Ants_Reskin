using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CrystallImage : MonoBehaviour
{
    [SerializeField] private Image _crystallImage;
    [SerializeField] private Vector2 _startScale = new Vector2(0.5f, 0.5f);

    public void SetImage(Sprite icon)
    {
        _crystallImage.sprite = icon;
    }

    public void Move(Vector3 firstTarget, Vector3 secondTarget, float firstDuration, float secondDuration)
    {
        transform.localScale = _startScale;
        _crystallImage.DOFade(1, firstDuration);
        transform.DOScale(Vector3.one, firstDuration).OnComplete(() => transform.DOScale(_startScale, secondDuration).SetEase(Ease.InQuad));
        float randomAngle = Random.Range(100f, 250f);
        transform.DORotate(new Vector3(0, 0, randomAngle), firstDuration * 0.8f);
        transform.DOMove(firstTarget, firstDuration).OnComplete(() => transform.DOMove(secondTarget, secondDuration).SetEase(Ease.InQuad)
                 .OnComplete(() => gameObject.SetActive(false)));
    }
}
