using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementAnimator : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;

    private Vector3 _startPoint;
    private Transform _targetPoint;
    private Vector3 _targetLocalPosition;
    private float _speed = 2.5f;
    private float _progress = 0;

    public void PlayCollectAnimation(Transform collectPoint, Transform newParent, Action finishAction = null)
    {
        _startPoint = transform.position;
        _targetPoint = collectPoint;
        _targetLocalPosition = _targetPoint.localPosition;

        StartCoroutine(WaitStopCollectAnimation(newParent, finishAction));
    }

    public void PlayDropAnimation(Transform target, Action OnAnimationComleted = null)
    {
        _startPoint = transform.position;
        _targetPoint = target;
        StartCoroutine(WaitStopDropAnimation(OnAnimationComleted));
    }

    private IEnumerator WaitStopCollectAnimation(Transform newParent, Action finishAction = null)
    {
        yield return new WaitForSeconds(0.1f);
        _trailRenderer.emitting = true;

        while (_progress < 1)
        {
            Vector3 target = _targetPoint.position - (_targetPoint.localPosition - _targetLocalPosition);
            List<Vector3> path = GetPath(target, target, _startPoint);
            MoveElement(path);
            yield return null;
        }

        transform.SetParent(newParent.transform);
        transform.localPosition = _targetLocalPosition;
        transform.localRotation = Quaternion.Euler(90, 50, 0);
        _trailRenderer.emitting = false;
        _progress = 0;
        finishAction?.Invoke();
    }

    private IEnumerator WaitStopDropAnimation(Action finishAction = null)
    {
        _trailRenderer.emitting = true;

        while (_progress < 1)
        {
            List<Vector3> path = GetPath(_startPoint, _targetPoint.position, _startPoint);
            MoveElement(path);
            yield return null;
        }
        _progress = 0;
        _trailRenderer.emitting = false;
        finishAction?.Invoke();
        gameObject.SetActive(false);
    }

    private void MoveElement(List<Vector3> path)
    {
        _progress += _speed * Time.deltaTime;
        transform.position = BezierCurve.Point3(_progress, path);
        transform.LookAt(BezierCurve.Point3(_progress + 0.1f, path));
    }

    private List<Vector3> GetPath(Vector3 maxPoint, Vector3 target, Vector3 startPoint)
    {
        float maxYOffset = maxPoint.y + 3f;
        Vector3 middlePoint = Vector3.Lerp(startPoint, target, 0.5f);
        middlePoint.y = maxYOffset;
        return new List<Vector3> { startPoint, middlePoint, target };
    }
}
