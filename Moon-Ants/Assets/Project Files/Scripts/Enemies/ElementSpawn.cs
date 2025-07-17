using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpawn : MonoBehaviour
{
    [SerializeField] private DeliveryDesk _deliveryDesk;

    private bool _isSpawn;

    public bool IsSpawn => _isSpawn;

    public void ChangedStatus(bool result)
    {
        _isSpawn = result;

        if (_isSpawn == false)
        {
            _deliveryDesk.AddFreePlace();
        }
    }
}
