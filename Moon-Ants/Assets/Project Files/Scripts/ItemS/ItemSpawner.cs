using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Item _item;
    [SerializeField] private PlayerBaza _baza;
    [SerializeField] private float _delayRespawn;

    private Item _currentItem;

    public void SpawnElement(Transform point, Vector3 offset)
    {
        StartCoroutine(StartRespawn(point, offset));
    }

    private IEnumerator StartRespawn(Transform point, Vector3 offset)
    {
        yield return new WaitForSeconds(_delayRespawn);

        _currentItem = Instantiate(_item, point.position, point.transform.rotation);
        _currentItem.GetSpawnPoint(point, this, offset);
        _baza.AddItem(_currentItem);
    }
}
