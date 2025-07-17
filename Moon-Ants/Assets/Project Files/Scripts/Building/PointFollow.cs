using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointFollow : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Update()
    {
        transform.position = _player.transform.position;
    }
}
