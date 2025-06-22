using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSaver : MonoBehaviour
{
    [SerializeField] private string _dataItemName;

    private ItemSavedData _itemSavedData;
    public string Name => _dataItemName;

    private void Awake()
    {
        if (_itemSavedData == null)
            _itemSavedData = new ItemSavedData(_dataItemName);
    }

    public void Save()
    {
        if(_itemSavedData == null)
            _itemSavedData = new ItemSavedData(_dataItemName);

        Data.SetItemsSavedDatas(_itemSavedData);
    }

    public void Load()
    {
        if (_itemSavedData == null)
            _itemSavedData = new ItemSavedData(_dataItemName);

        ItemSavedData itemSavedData = Data.GetItemSavedDatas(_itemSavedData);

        Debug.Log("Load() " + gameObject + " _dataItemName" + _dataItemName);
            
        if (itemSavedData == null)
        {
            Save();
            return;
        }
        Debug.Log("Load() " + gameObject + " _itemSavedData" + _itemSavedData.Name);

        _itemSavedData = itemSavedData;
    }

    public void SetIndexItem(int index)
    {
        _itemSavedData.IndexItem = index;
    }

    public int GetIndexItem()
    {
        return _itemSavedData.IndexItem;
    }

    public void ResetIndex()
    {
        //_itemSavedData.IndexItem = -1;
        //Save();
    }
}

[Serializable]
public class ItemSavedData
{
    public string Name;
    public int IndexItem = -1;

    public ItemSavedData(string dataItemName)
    {
        Name = dataItemName;
    }
}
