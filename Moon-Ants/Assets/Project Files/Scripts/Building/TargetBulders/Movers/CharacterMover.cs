using UnityEngine;
using DG.Tweening;

public class CharacterMover : MonoBehaviour
{
    [SerializeField] private Transform[] _targets;
    [SerializeField] private float _moveDuration;
    [SerializeField] private float _rotateDuration;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;

    private Tweener _moveTweener;
    private Vector3[] _path;

    public void Move()
    {
        if (_moveTweener != null && _moveTweener.IsActive() == true)
            _moveTweener.Kill();

        _moveTweener = transform.DOMove(new Vector3(_targets[0].position.x, transform.position.y, _targets[0].position.z), _moveDuration).SetEase(Ease.Linear).OnComplete(() => StopAnimation());
        transform.DOLookAt(_targets[0].position, _rotateDuration, AxisConstraint.Y).SetEase(Ease.Linear);
        _animator.SetBool(AnimatorConst.Run, true);
        _audioSource.Play();
    }

    public void MoveToPatch(Ease ease = Ease.Linear)
    {
        if (_moveTweener != null && _moveTweener.IsActive() == true)
            _moveTweener.Kill();
        _path = new Vector3[_targets.Length];
        for (int i = 0; i < _path.Length; i++)
        {
            _path[i] = _targets[i].position;
        }

        _moveTweener = transform.DOPath(_path, _moveDuration, PathType.CatmullRom).SetEase(ease).SetLookAt(0.1f).OnComplete(() => StopAnimation());

        if (_animator != null)
            _animator.SetBool(AnimatorConst.Run, true);
    }

    public void LoopsRotate(Transform target, Vector3 endValue, float duration)
    {
        target.DOLocalRotate(endValue, duration).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopAnimation()
    {
        if (_animator != null)
            _animator.SetBool(AnimatorConst.Run, false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.position + transform.forward * 3f);
    }
}
