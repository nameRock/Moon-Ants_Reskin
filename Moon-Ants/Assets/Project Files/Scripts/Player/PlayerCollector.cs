using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    [SerializeField] private float _delayCollect = 0.6f;
    [SerializeField] private float _durationScale = 0.5f;
    [SerializeField] private int _lengthStack;
    [SerializeField] private float _durationMove;
    [SerializeField] private float _dropDelay;
    [SerializeField] private Transform _collectPoint;
    [SerializeField] private GameObject _boneStack;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Baza _baza;
    [SerializeField] private ItemFinder _itemFinder;
    [SerializeField] private GameObject _textMax;
    [SerializeField] private Animator _animator;
    [SerializeField] private Fabrica _fabrica;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private PlayerMover _playerMover;

    private float _currentTime;
    private bool _isCollect;
    private bool _isDisableCollected;
    private int _currentCountElement;
    private int _stackLenghtStep = 0;
    private int _startStackLenght;

    private int _maxStackLenght => _lengthStack + _stackLenghtStep;
    private Item _currentItem;
    private Element _currentElement;
    private List<Element> _elements;
    private Vector3 _startPoint;
    private Coroutine _dropCoroutine;

    private bool _isDrop = false;

    public Baza Baza => _baza;
    public bool IsCollect => _isCollect;

    public int ElementsCount => _elements.Count;
    public bool ContainsEnerge => _elements.Where(element => element.IsEnerge == true && element.IsGreen == false).Count() > 0;

    public event Action CollectingStarted;
    public event Action CollectingEnded;
    public event Action DropingStarted;
    public event Action DropingEnded;
    public event Action DroppedErrorEnergy;
    public event Action DroppedErrorFood;

    private void Awake()
    {
        _startStackLenght = _lengthStack;
        _elements = new List<Element>();
        _startPoint = _collectPoint.transform.localPosition;
#if UNITY_EDITOR
        Debug.Log("Stack");
        _lengthStack = 30;
        //_delayCollect = 0;
        //_dropDelay = 0;
#endif
    }

    private void OnEnable()
    {
        _itemFinder.Finded += Collect;
        _itemFinder.Lossed += DisCollect;
    }

    private void OnDisable()
    {
        _itemFinder.Finded -= Collect;
        _itemFinder.Lossed -= DisCollect;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        float currentDelayCollect = _delayCollect * GetDelayMultiplier(); ;
        if (_isCollect)
        {
            if (_currentTime >= currentDelayCollect)
            {
                Element targetElement = _currentItem.GetNextElement();
                _currentItem.ActivateHelper();

                if (targetElement == null)
                {
                    StopCollecting();
                    return;
                }

                _currentTime = 0;
                _currentCountElement++;

                _animator.SetTrigger(AnimatorConst.Dig);

                CollectElement(targetElement);

                if (_currentCountElement >= _maxStackLenght)
                {
                    StopCollecting();
                    _isDisableCollected = true;
                }
            }
        }
    }


    public void Drop(Transform target, bool isBaza, bool isFabrica = false, bool isOnlyEnergy = false)
    {
        if (isOnlyEnergy && ContainsEnerge == false)
            return;

        if (_dropCoroutine != null)
        {
            StopCoroutine(_dropCoroutine);
            _isDrop = false;
        }

        _dropCoroutine = StartCoroutine(StartDrop(target, isBaza, isFabrica, isOnlyEnergy));
    }

    public void StopDrop()
    {
        if (_dropCoroutine != null)
        {
            StopCoroutine(_dropCoroutine);
            _isDrop = false;
            if (_elements.Count == 0)
            {
                _animator.SetBool(AnimatorConst.IsCollect, false);
            }
            DropingEnded?.Invoke();
        }
    }

    public void SetStackIncreasedStep(int step)
    {
        _stackLenghtStep = step;
        if (_currentCountElement < _maxStackLenght)
        {
            _isDisableCollected = false;
            _textMax.SetActive(false);
        }
    }
    public void ChangedLengthStack(int lvl)
    {
        _lengthStack += (lvl - 1);
    }

    private void Collect(Item item)
    {
        if (_isDisableCollected == false && _isCollect == false)
        {
            _currentItem = item;
            _isCollect = true;
            _animator.SetBool(AnimatorConst.IsCollect, true);
            CollectingStarted?.Invoke();
        }
    }

    private void DisCollect()
    {
        StopCollecting();
    }

    public void UpgradeLengthStack()
    {
        _lengthStack++;
    }

    private void StopCollecting()
    {
        CollectingEnded?.Invoke();
        _isCollect = false;
    }
    public void UnbendPose()
    {
        _animator.SetBool(AnimatorConst.IsCollect, false);
    }

    public void SetDefaultPose()
    {
        _animator.SetBool(AnimatorConst.IsCollect, _elements.Any());
    }

    private IEnumerator StartDrop(Transform target, bool isBaza, bool isFabrica = false, bool isOnlyEnergy = false)
    {
        _isDrop = true;
        if (_currentCountElement != _elements.Count)
            throw new Exception("Not valid _currentCountElement value");
        DropingStarted?.Invoke();

        int startElementCount = _elements.Count;
        int processedElementCount = 0;
        for (int i = _elements.Count - 1; i >= 0; i--)
        {
            if (_isDrop == false)
                break;

            processedElementCount++;
            Action onDropComleted = null;
            if (isBaza)
            {
                if (isOnlyEnergy == true && _elements[i].IsEnerge == false)
                    continue;

                if (isOnlyEnergy == true && _elements[i].IsGreen == true)
                    continue;

                if (_elements[i].IsEnerge)
                    _baza.AddValueWallet(_elements[i], "Player");
                else
                {
                    DroppedErrorFood?.Invoke();
                    continue;
                }
            }
            else if (_fabrica != null && isFabrica)
            {
                if (_elements[i].IsEnerge && _elements[i].IsGreen == false && _fabrica.IsReady == true)
                {
                    _fabrica.AddElement(this);
                    onDropComleted = _fabrica.ActivateFreeElement;
                }
                else
                    continue;
            }
            else
            {
                if (_elements[i].IsEnerge == false)
                    _baza.AddValueWallet(_elements[i], "Player");
                else
                {
                    DroppedErrorEnergy?.Invoke();
                    continue;
                }
            }
            _audioSource.Play();

            _collectPoint.localPosition -= _offset;
            _currentCountElement--;

            _isDisableCollected = false;

            _textMax.SetActive(false);

            Element element = _elements[i];
            float delay = _dropDelay * GetDelayMultiplier();
            _elements.Remove(_elements[i]);

            element.PlayDrop(target, onDropComleted);
            CalculateElementPosition(startElementCount, processedElementCount);
            element.BaseElement.DisableElement();

            yield return new WaitForSeconds(delay);
        }
        if (_elements.Count == 0)
        {
            _animator.SetBool(AnimatorConst.IsCollect, false);
        }
        DropingEnded?.Invoke();
    }

    private void CollectElement(Element element)
    {
        Element newElement = Instantiate(element.GetTemplateElement(), element.transform.position, Quaternion.Euler(90, 0, 0));
        newElement.Init(element);
        element.gameObject.SetActive(false);

        newElement.PlayCollect(_collectPoint, _boneStack.transform, PlayFinishCollectingAction);

        _currentElement = newElement;
        _collectPoint.localPosition += _offset;
        _elements.Add(_currentElement);
        _currentItem.CheckLastElements(element);
    }

    private void PlayFinishCollectingAction()
    {
        _audioSource.Play();

        if (_isDisableCollected == true)
            _textMax.SetActive(true);
    }

    private void CalculateElementPosition(int startElementCount, int processedElementCount)
    {
        int deletedElementCount = startElementCount - _currentCountElement;
        int differenceCount = processedElementCount - deletedElementCount;
        for (int j = _elements.Count - 1; j > _elements.Count - 1 - differenceCount; j--)
        {
            _elements[j].transform.localPosition -= _offset;
        }
    }

    private float GetDelayMultiplier()
    {
        return (float)_startStackLenght / _maxStackLenght;
    }
}
