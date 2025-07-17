using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private int _countElement;

    public int CountElement => _countElement;

    private void Start()
    {
        ChangedText();
    }

    public void ChangedCountElement()
    {
        _countElement--;
        ChangedText();
    }

    public void GetCountElement(int count)
    {
        _countElement -= count;
        ChangedText();
    }

    private void ChangedText()
    {
        if (_countElement < 0)
            _countElement = 0;

        _text.text = "x" + _countElement.ToString();
    }
}
