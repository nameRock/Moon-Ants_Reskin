using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMover : MonoBehaviour
{
    private const string Speed = "Speed";
    private const string Run = "IsRun";
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";

    [SerializeField] private Joystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    [SerializeField] private float _durationReturn;
    [SerializeField] private GameObject _virtualCamera;
    [SerializeField] private MoveType _moveType = MoveType.NonGroundCheking;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private SkinMovableElement _movableElement;
    [SerializeField] private GameObject _maxObject;

    private Rigidbody _rigidbody;
    private Camera _camera;
    private CapsuleCollider _capsuleCollider;
    private float _speedMultiplier = 1f;
    private bool _isRun = true;
    private bool _isDesktopInput = false;

    public float InputDirectionMagnitude { get; private set; } = 0;

    private void Awake()
    {
        _isDesktopInput = Application.isMobilePlatform == false;
        _rigidbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _camera = Camera.main;
    }
   
    private void FixedUpdate()
    {
        if (_isRun == false)
        {
            return;
        }        

        Vector2 inputDirection = GetInputDirection();
        InputDirectionMagnitude = inputDirection.magnitude;
        _animator.SetFloat(Speed, InputDirectionMagnitude);

        if (inputDirection == Vector2.zero)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }

        Vector3 direction = GetRotatedDirection(inputDirection, _camera.transform.rotation.eulerAngles.y);
        Rotate(direction);

        if (_moveType == MoveType.GroundChecking)
        {
            bool isGrounded = _groundChecker.TryCheckTarget(out float corretedAngle);
            if (isGrounded == false)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }
            if (corretedAngle != 0)
            {
                direction = GetRotatedDirection(new Vector2(direction.x, direction.z), corretedAngle);
            }
        }

        _rigidbody.velocity = new Vector3(direction.x * _speed * _speedMultiplier, _rigidbody.velocity.y, direction.z * _speed * _speedMultiplier);
    }

    private void OnDisable()
    {
        if (_movableElement != null)
            _movableElement.gameObject.SetActive(false);
    }

    public void SetSpeedMultiplier(float speedMultiplier)
    {
        _speedMultiplier = speedMultiplier;
    }

    public void SetJoystickState(bool isActive)
    {
        _isRun = isActive;
        _joystick.enabled = isActive;
        _joystick.gameObject.SetActive(isActive);
    }

    public void SetState(bool isActive)
    {
        SetJoystickState(isActive);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = isActive == false;
        _animator.SetFloat(Speed, 0);
    }

    public void DisableRigidbody(bool isResult)
    {
        _rigidbody.isKinematic = isResult;
    }

    private Vector2 GetInputDirection()
    {
        if (_joystick.Direction != Vector2.zero || _isDesktopInput == false)
            return _joystick.Direction.normalized;
        return new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical)).normalized;
    }

    private Vector3 GetRotatedDirection(Vector2 direction, float corectedAngle)
    {
        float angle = -corectedAngle / Mathf.Rad2Deg;
        float rotatedX = direction.x * (float)Math.Cos(angle) - direction.y * (float)Math.Sin(angle);
        float rotatedZ = direction.x * (float)Math.Sin(angle) + direction.y * (float)Math.Cos(angle);
        return new Vector3(rotatedX, 0, rotatedZ);
    }

    private void Rotate(Vector3 forward)
    {
        float rotationSpeed = 12f;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(forward), rotationSpeed);
    }

    public void MoveToFinish(Transform target)
    {
        SetState(false);
        StartFinishMove();

        transform.DOLookAt(target.position, 0, AxisConstraint.Y);
        transform.DOMove(new Vector3(target.position.x, transform.position.y, target.position.z) + new Vector3(0, 0, 0), 
            _durationReturn).SetEase(Ease.Linear).OnComplete(() => DisablePlayer());
    }

    public void MoveToPath(Vector3[] targets)
    {
        SetState(false);
        StartFinishMove();

        transform.DOPath(targets, _durationReturn).SetLookAt(0.1f).SetEase(Ease.Linear).
            OnComplete(StopMoveToPath);
    }

    private void DisablePlayer()
    {
        gameObject.SetActive(false);
        _maxObject.SetActive(false);
    }

    private void StopMoveToPath()
    {
        transform.DOLookAt(Camera.main.transform.position, 0.5f, AxisConstraint.Y).SetEase(Ease.Linear);
        _animator.SetBool(Run, false);
    }

    private void StartFinishMove()
    {
        _virtualCamera.SetActive(false);

        _animator.SetBool(Run, true);

        _capsuleCollider.enabled = false;
    }

    public void ChangedSpeed(int lvl)
    {
        _speed += (0.05f * lvl);
    }

    public void StopRun()
    {
        _animator.SetBool(Run, false);
        _animator.SetFloat(Speed, 0);
        _rigidbody.isKinematic = true;
    }

    private enum MoveType
    {
        NonGroundCheking,
        GroundChecking
    }
}
