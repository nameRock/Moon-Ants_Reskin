using DG.Tweening;
using UnityEngine;

public class AntCollector : MonoBehaviour
{
    [SerializeField] private float _durationScale;
    [SerializeField] private Transform _collectPoint;

    private Element _currentElement;
    private Baza _baza;
    private bool _isCollect;

    public void GetBaza(Baza baza)
    {
        _baza = baza;
    }

    public void SetElement(Element element)
    {
        _currentElement = element;
        ChangedScaleElement(element);
    }

    public void SetDurationScale(float durationScale)
    {
        _durationScale = durationScale;
    }

    private void ChangedScaleElement(Element element)
    {
        element.transform.DOScale(0, _durationScale).SetEase(Ease.Linear).OnComplete(() => Collect());
    }

    private void Collect()
    {
        _currentElement.transform.position = _collectPoint.position;
        _currentElement.transform.SetParent(transform);
        _currentElement.transform.DOScale(_currentElement.Scale, 0);

        _isCollect = true;

        _currentElement.StopParticls();
    }

    public void Drop()
    {
        if (_currentElement != null)
        {
            //_currentElement.gameObject.SetActive(false);
            _currentElement.DisableElement();
            if (_baza is PlayerBaza playerBaza)
            {
                if (_isCollect)
                {
                    _isCollect = false;
                    playerBaza.AddValueWallet(_currentElement, "Ant");
                }
            }
            _currentElement = null;
        }
    }
}
