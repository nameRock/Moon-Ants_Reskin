using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SkinMovableElement : SkinElement
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Transform _animationPoint;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _speed;
    [SerializeField] private float _verticalOffset = 2;

    private Tween _swingTween;

    private Coroutine _wairStartAnimation;

    public override void TryAcivate(Skin skin)
    {
        SetState(skin);
        if (IsActive == false)
            return;

        transform.SetParent(null);
        KillTween();
        if (_wairStartAnimation != null)
        {
            StopCoroutine(_wairStartAnimation);
        }
        _wairStartAnimation = StartCoroutine(WaitStartAnimation());
    }

    private void Update()
    {
        Vector3 direction = _targetPoint.position - transform.position;

        if (Vector3.SqrMagnitude(direction) > 0.2f)
            transform.position = Vector3.Lerp(transform.position, _targetPoint.position, _speed * Time.deltaTime);

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 150 * Time.deltaTime);
    }

    private IEnumerator WaitStartAnimation()
    {
        enabled = false;
        _animationPoint.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _animationPoint.gameObject.SetActive(true);
        transform.position = _startPoint.position;
        transform.LookAt(_targetPoint);
        yield return new WaitForSeconds(0.1f);

        KillTween();
        enabled = true;
        _animationPoint.localPosition = Vector3.zero;   
        _swingTween = _animationPoint.DOLocalMoveY(_verticalOffset, 2f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        KillTween();
        enabled = false;
    }

    private void KillTween()
    {
        if (_swingTween != null && _swingTween.IsActive() == true)
        {
            _swingTween.Kill();
        }
    }
}
