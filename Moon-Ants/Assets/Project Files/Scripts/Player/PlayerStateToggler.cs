using System.Collections;
using UnityEngine;

public class PlayerStateToggler : MonoBehaviour
{
    private const string IsDied = "IsDied";

    [SerializeField] private OxygenDetector _oxygenDetector;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private float _revivedDelay = 3f;

    private Vector3 _startPosition;

    private void Awake()
    {
        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        _oxygenDetector.FullOxygenLossed += OnFullOxygenLossed;
    }

    private void OnDisable()
    {
        _oxygenDetector.FullOxygenLossed -= OnFullOxygenLossed;
    }

    private void OnFullOxygenLossed()
    {
        KillPlayer();
        StartCoroutine(WaitRevivalPlayer());
    }

    private void KillPlayer()
    {
        _animator.SetBool(IsDied, true);
        _playerMover.DisableRigidbody(true);
        _playerMover.SetJoystickState(false);
        _playerMover.enabled = false;
    }

    private IEnumerator WaitRevivalPlayer()
    {
        yield return new WaitForSeconds(_revivedDelay);
        RevivePlayer();
    }

    private void RevivePlayer()
    {
        _oxygenDetector.Reset();
        _animator.SetBool(IsDied, false);
        _playerMover.enabled = true;
        _playerMover.SetJoystickState(true);
        _playerMover.DisableRigidbody(false);
        _playerMover.transform.position = _startPosition;
    }
}
