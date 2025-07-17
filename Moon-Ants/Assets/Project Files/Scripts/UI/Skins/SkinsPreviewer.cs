using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class SkinsPreviewer : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Player _player;
    [SerializeField] private Transform _rotationPoint;
    [SerializeField] private int _minPriority = 0;
    [SerializeField] private int _maxPriority = 20;

    public void Open()
    {
        _virtualCamera.Priority = _maxPriority;
        _player.PlayerMover.enabled = false;
        _player.PlayerCollector.UnbendPose();
        _player.transform.DOLookAt(_rotationPoint.position, 0.5f, AxisConstraint.Y);
    }

    public void Close()
    {
        _virtualCamera.Priority = _minPriority;
        _player.PlayerMover.enabled = true;
        _player.PlayerCollector.SetDefaultPose();
    }
}
