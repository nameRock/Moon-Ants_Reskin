using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketElement : MonoBehaviour
{
    [SerializeField] private float _finishScaleX = 1;
    [SerializeField] private float _finishScaleY = 1;
    [SerializeField] private float _finishScaleZ = 1;

    private void OnEnable()
    {
        transform.DOScaleX(1.5f, 0.25f).SetEase(Ease.Linear).OnComplete(() => transform.DOScaleX(_finishScaleX, 0.25f).SetEase(Ease.Linear));
        transform.DOScaleY(1.5f, 0.25f).SetEase(Ease.Linear).OnComplete(() => transform.DOScaleY(_finishScaleY, 0.25f).SetEase(Ease.Linear));
        transform.DOScaleZ(1.5f, 0.25f).SetEase(Ease.Linear).OnComplete(() => transform.DOScaleZ(_finishScaleZ, 0.25f).SetEase(Ease.Linear));
    }
}
