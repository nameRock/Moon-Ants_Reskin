using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderUpgrade : MonoBehaviour
{
    [SerializeField] private Upgrader _coutUpgrader;
    [SerializeField] private Upgrader _speedUpgrader;
    [SerializeField] private Upgrader _forceUpgrader;
    [SerializeField] private Upgrader _playerSpeedUpgrader;
    [SerializeField] private Upgrader _playerPriceUpgrader;
    [SerializeField] private Upgrader _playerStackUpgrader;
    [SerializeField] private Data _data;

    private void OnEnable()
    {
        if(Data.IsSeted == true)
        {
            SetSavedData();
            return;
        }
        Data.Setted += OnDataSeted;
    }

    private void OnDataSeted()
    {
        SetSavedData();
        Data.Setted -= OnDataSeted;
    }

    private void SetSavedData()
    {
        _coutUpgrader.GetLevelUpgrade(_data.GetCountUpgrade());
        _speedUpgrader.GetLevelUpgrade(_data.GetSpeedUpgrade());
        _forceUpgrader.GetLevelUpgrade(_data.GetForceUpgrade());

        if(_playerPriceUpgrader != null)
        {
            _playerPriceUpgrader.GetLevelUpgrade(_data.GetPlayerPriceUpgrade());
        }
        if(_playerSpeedUpgrader != null)
        {
            _playerSpeedUpgrader.GetLevelUpgrade(_data.GetPlayerSpeedUpgrade());
        }
        if(_playerStackUpgrader != null)
        {
            _playerStackUpgrader.GetLevelUpgrade(_data.GetPlayerStackUpgrade());
        }
    }
}
