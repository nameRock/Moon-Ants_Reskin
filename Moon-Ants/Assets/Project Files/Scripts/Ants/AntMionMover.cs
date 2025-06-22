using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AntMionMover : MonoBehaviour
{
    private const string Dig = "IsDig";

    [SerializeField] private float _durationMove;
    [SerializeField] private float _durationReturn;
    [SerializeField] private float _delay;
    [SerializeField] private Animator _animator;
    [SerializeField] private MoveType _pathType = MoveType.Linear;
    [SerializeField, Range(0, 0.1f)] private float _curvatureMultiplier = 0.05f;
    [SerializeField] private bool _isBonus = false;
    [SerializeField] private ParticleSystem _buf;

    private Baza _baza;
    private Element _currentElemet;
    private Item _currentItem;
    private Vector3 _currentTarget;
    private Vector3 _currentOffset;
    private bool _isHome = false;
    private bool _isGoToBase = true;
    private float _currentDelay;
    private AntCollector _antCollector;
    private Tweener _moveTweener;
    private bool _isRun = true;
    private Coroutine _delayCoroutine;
    private TargetBuilder _rocket;
    private bool _isOne = true;

    public event Action<AntMionMover> Died;

    public Item CurrentItem => _currentItem;

    private void Awake()
    {
        _antCollector = GetComponent<AntCollector>();
    }

    public void SetTarget(TargetBuilder rocketBuilder)
    {
        _rocket = rocketBuilder;
    }

    public void StartBehavior(Baza baza)
    {
        _baza = baza;
        _antCollector.GetBaza(baza);
        GetNextTarget();
    }

    public void PlayBuf()
    {
        if (_buf != null)
        {
            _buf.Play();
        }
    }

    public void GetNextTarget()
    {
        if (_isRun == false)
            return;

        if (_isHome == false)
        {
            GotToTheBase();
        }
        else
        {
            _isGoToBase = _currentElemet.IsEnerge;

            if (_isOne == false)
            {
                if (_currentElemet.IsEnerge == false)
                {
                    ChangedParams(false, 0, new Vector3(0, 0, 0));

                    _antCollector.Drop();
                    _isGoToBase = true;

                    _isOne = true;
                }
            }
            else
            {
                _animator.SetBool(Dig, true);
                _currentItem.CheckLastElements(_currentElemet);
                ChangedParams(false, _delay, new Vector3(0, 0, 0));
            }

            _antCollector.SetElement(_currentElemet);

            _currentTarget = _baza.transform.position;

            _delayCoroutine = StartCoroutine(StartDelay(_currentDelay));
            //StartCoroutine(StartChekCollectedElement(_currentDelay));
        }

    }

    private void GotToTheBase()
    {
        if (_isGoToBase)
        {
            if (_currentElemet != null)
            {
                if (_currentElemet.IsEnerge)
                {
                    _antCollector.Drop();

                    if (_isBonus)
                    {
                        _baza.RemoveBonusAnt(this);
                        return;
                    }
                }
                else
                {
                    if (_isBonus)
                    {
                        _baza.RemoveBonusAnt(this);
                        return;
                    }
                }
            }
            GetNextElement();
        }
        else
        {
            MoveToTargetBuilder();
        }
    }

    private void MoveToTargetBuilder()
    {
        _isOne = false;
        _currentTarget = _rocket.AntsPoint.position; //_rocket.transform.position;
        _isGoToBase = true;
        ChangedParams(true, 0, new Vector3(0, 0, 0));
        StartMove();
    }

    private void GetNextElement()
    {
        _currentItem = _baza.GetCurrentItem();

        if (_currentItem != null)
        {
            _currentElemet = _currentItem.GetNextElement();

            if (_currentElemet != null)
            {
                _currentTarget = _currentElemet.transform.position;

                ChangedParams(true, 0, _currentItem.Offset);

                _delayCoroutine = StartCoroutine(StartDelay(_currentDelay));
            }
            else
            {
                GetNextTarget();
            }
        }
    }

    public void Kill()
    {
        if (_currentElemet != null)
            _currentElemet.gameObject.SetActive(false);

        Died?.Invoke(this);
        Destroy(gameObject);
    }

    public void SetDelay(float delay)
    {
        _delay = delay;
        _antCollector.SetDurationScale(_delay);
    }

    public void SetDurationMove(float durationMove)
    {
        _durationMove = durationMove;
        if (_moveTweener != null && _moveTweener.active == true)
        {
            StartMove();
        }
    }

    private void ChangedParams(bool isHome, float delay, Vector3 offset)
    {
        _isHome = isHome;
        _currentDelay = delay;
        _currentOffset = offset;
    }

    private IEnumerator StartDelay(float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        _animator.SetBool(Dig, false);
        StartMove();
    }

    private IEnumerator StartChekCollectedElement(float delay)
    {
        yield return new WaitForSeconds(delay); //////////////////
        _currentItem.CheckLastElements(_currentElemet);           //////////////////
    }

    private void StartMove()
    {
        float distantion = Vector3.Distance(transform.position, _currentTarget);
        if (_moveTweener != null && _moveTweener.active == true)
        {
            _moveTweener.Kill();
        }
        Move(_currentTarget, distantion);
    }

    public void StopMove()
    {
        _isRun = false;
        _moveTweener.Kill();
        _animator.SetBool(Dig, true);
    }

    public void MoveToFinish(Transform target)
    {
        _isRun = false;

        if (_delayCoroutine != null)
            StopCoroutine(_delayCoroutine);

        _moveTweener.Kill();
        _currentTarget = target.position;
        float distantion = Vector3.Distance(transform.position, _currentTarget);

        transform.DOMove(new Vector3(_currentTarget.x, transform.position.y, _currentTarget.z) + new Vector3(0, 0, 0), _durationReturn).SetEase(Ease.Linear).OnComplete(() => DisableAtn());
        if (distantion > 0.5f)
            transform.DOLookAt(_currentTarget, 0, AxisConstraint.Y);
    }

    private void Move(Vector3 target, float distantion)
    {
        if (_isRun == false)
            return;

        if (_pathType == MoveType.Linear)
        {
            Vector3 correctedTarget = new Vector3(target.x, transform.position.y, target.z) + _currentOffset;
            _moveTweener = transform.DOMove(correctedTarget, distantion / _durationMove).SetEase(Ease.Linear).OnComplete(() => GetNextTarget());
            transform.DOLookAt(target, 0, AxisConstraint.Y);
        }
        else
        {
            Vector3[] path = CreateCurvedPath(target, distantion);
            _moveTweener = transform.DOPath(path, distantion / _durationMove, PathType.CatmullRom).SetLookAt(0.01f).SetEase(Ease.Linear);
            _moveTweener.OnComplete(() => GetNextTarget());
        }
    }

    private Vector3[] CreateCurvedPath(Vector3 target, float distantion)
    {
        target = new Vector3(target.x, transform.position.y, target.z) + _currentOffset;
        Vector3 direction = (target - transform.position).normalized;
        int sign = GetRandomSign();
        float rotatedAngle = 90;
        Vector3 point1 = Vector3.Lerp(transform.position, target, Random.Range(0.25f, 0.35f)) + GetRotatedDirection(direction, rotatedAngle * sign) * (distantion * _curvatureMultiplier) * Random.Range(0.5f, 1f);
        Vector3 point2 = Vector3.Lerp(transform.position, target, Random.Range(0.6f, 0.7f)) + GetRotatedDirection(direction, rotatedAngle * -sign) * (distantion * _curvatureMultiplier) * Random.Range(0.5f, 1f);
        return new Vector3[] { point1, point2, target };
    }

    private int GetRandomSign()
    {
        int randomValue = Random.Range(0, 2);
        if (randomValue == 0)
            return -1;
        return 1;
    }

    private Vector3 GetRotatedDirection(Vector3 direction, float angle)
    {
        angle /= Mathf.Rad2Deg;
        float rotatedX = direction.x * (float)Math.Cos(angle) - direction.z * (float)Math.Sin(angle);
        float rotatedZ = direction.x * (float)Math.Sin(angle) + direction.z * (float)Math.Cos(angle);
        return new Vector3(rotatedX, 0, rotatedZ);
    }

    private void DisableAtn()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _moveTweener.Kill();
    }

    private enum MoveType
    {
        Linear,
        Curve
    }
}
