using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalChecker : MonoBehaviour
{
    [SerializeField] private Item _cristal1;
    [SerializeField] private Item _cristal2;
    [SerializeField] private OxygenZone[] _oxygenZones;
    [SerializeField] private int _price;
    [SerializeField] private Wallet _wallet;

    private bool _isOpen = false;

    private void Update()
    {
        if (!_isOpen)
        {
            if(_cristal1.IsCompleted && _cristal2.IsCompleted && _wallet.BlueCountValue < _price)
            {
                _isOpen = true;
                OpenAllZone();
            }
        }
    }

    private void OpenAllZone()
    {
        for (int i = 0; i < _oxygenZones.Length; i++)
        {
            _oxygenZones[i].OpenFree();
        }
    }
}
