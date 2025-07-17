using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    [SerializeField] private GameObject _arrowHelper1;
    [SerializeField] private GameObject _arrowHelper2;
    [SerializeField] private GameObject _arrowHelper3;
    [SerializeField] private GameObject _helper;
    [SerializeField] private bool _isTimer = false;
    [SerializeField] private bool _isStartDelay = false;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    private IEnumerator Start()
    {
        if (_isStartDelay == true)
        {
            float startDelay = 3f;
            yield return new WaitForSeconds(startDelay);
            _arrowHelper2.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerCollector collector))
        {
            _sphereCollider.enabled = false;

            if (_isTimer)
            {
                StartCoroutine(StartTimer(7));
            }
            else
            {
                StartCoroutine(StartTimer(0));
            }

            if (_arrowHelper3 != null)
            {
                _arrowHelper3.SetActive(true);
            }

            if (_arrowHelper2 != null)
            {
                _arrowHelper2.SetActive(false);
            }
        }
    }

    private IEnumerator StartTimer(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_arrowHelper1 != null)
        {
            _arrowHelper1.SetActive(true);
        }

        if (_helper != null)
        {
            _helper.SetActive(true);
        }
    }
}
