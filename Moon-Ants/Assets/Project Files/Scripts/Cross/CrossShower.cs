using System.Collections;
using UnityEngine;

public class CrossShower : MonoBehaviour
{
    [SerializeField] private PlayerCollector _playerCollector;
    [SerializeField] private GameObject _modelObjectFood;
    [SerializeField] private GameObject _modelObjectEnergy;

    private Coroutine _ditain;
    private float _time = 3f;

    private void OnEnable()
    {
        _playerCollector.DroppedErrorEnergy += OnDroppEnergy;
        _playerCollector.DroppedErrorFood += OnDropFood;
    }

    private void OnDisable()
    {
        _playerCollector.DroppedErrorEnergy -= OnDroppEnergy;
        _playerCollector.DroppedErrorFood -= OnDropFood;
    }

    private void OnDropFood()
    {
        _modelObjectFood.SetActive(true);

        if (_ditain == null)
            _ditain = StartCoroutine(Ditain(_time));
    }

    private void OnDroppEnergy()
    {
        _modelObjectEnergy.SetActive(true);

        if (_ditain == null)
            _ditain = StartCoroutine(Ditain(_time));
    }

    private IEnumerator Ditain(float time)
    {
        var dilay = new WaitForSeconds(time);
        yield return dilay;
        _modelObjectFood.SetActive(false);
        _modelObjectEnergy.SetActive(false);
        StopCoroutine(_ditain);
        _ditain = null;
    }
}
