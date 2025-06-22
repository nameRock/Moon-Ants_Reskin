using DG.Tweening;
using System.Linq;
using UnityEngine;

public class DeliveryDesk : MonoBehaviour
{
    [SerializeField] private ElementSpawn[] _elementSpawn;
    [SerializeField] private Element _template;

    private Item _item;
    private int _numFreePlace;

    private void Awake()
    {
        _item = GetComponent<Item>();
        _numFreePlace = _elementSpawn.Length;
    }

    public void SpawnElement()
    {
        for (int i = 0; i < _elementSpawn.Length; i++)
        {
            if (_elementSpawn[i].IsSpawn == false)
            {               
                Element element = Instantiate(_template, _elementSpawn[i].transform.position, Quaternion.Euler(new Vector3(90, 0, 0)), transform);
                _item.AddElement(element);
                element.SetItem(_item);
                element.SetSpawnPoint(_elementSpawn[i]);
                _elementSpawn[i].ChangedStatus(true);
                _numFreePlace--;
                element.EnableCollider();                
                break;
            }
        }
    }

    public void AddFreePlace()
    {
        _numFreePlace++;
    }

    public bool CheckFreePlace()
    {
        return _numFreePlace > 0;
    }
}
