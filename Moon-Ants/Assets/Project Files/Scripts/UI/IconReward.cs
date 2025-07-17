using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IconReward : MonoBehaviour
{
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private float _time = 60;

    private float _currentTime;

    private void Start()
    {
        _timer.text = "1:00";
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        if(_currentTime >= 1)
        {
            _currentTime = 0;
            _time -= 1f;
            ChangedTimer();

            if(_time <= 0)
            {
                _time = 60;
                _timer.text = "1:00";
                gameObject.SetActive(false);
            }
        }
    }

    private void ChangedTimer()
    {
        if(_time >= 10)
        {
            _timer.text = "0:" + _time;
        }
        else
        {
            _timer.text = "0:" + "0" + _time;
        }
    }
}
