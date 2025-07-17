using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaSaver : MonoBehaviour
{
    [SerializeField] private string _zonaName;

    private SaveZona _saveZona;

    private void Awake()
    {
        if (_saveZona == null)
            _saveZona = new SaveZona(_zonaName);
    }

    public void Save()
    {
        if (_saveZona == null)
            _saveZona = new SaveZona(_zonaName);

        Data.SetSavedZona(_saveZona);
    }

    public void Load()
    {
        if (_saveZona == null)
            _saveZona = new SaveZona(_zonaName);

        SaveZona savedZona = Data.GetSavedZona(_saveZona);

        if (savedZona == null)
        {
            Save();
            return;
        }

        _saveZona = savedZona;
    }

    public void SetValueZone(bool result)
    {
        _saveZona.IsOpen = result;
    }

    public bool GetValueZone()
    {
        return _saveZona.IsOpen;
    }

    public void ResetOpening()
    {
        _saveZona.IsOpen = false;
        Save();
    }
}

[Serializable]
public class SaveZona
{
    public string Name;
    public bool IsOpen;

    public SaveZona(string name)
    {
        Name = name;
        IsOpen = false;
    }
}
