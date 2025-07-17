using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class SkinsSelector : MonoBehaviour
{
    [SerializeField] private SkinData[] _skinsDatas;
    [SerializeField] private Data _data;
    [SerializeField] private Skin[] _skins;
    [SerializeField] private SkinsVisualizer _visualizer;

    private Wallet _wallet;
    private Player _player;

    public Skin CurrentSkin { get; private set; }

    public IReadOnlyCollection<Skin> Skins => _skins;
    public SkinsVisualizer Visualizer => _visualizer;

    public event Action SkinsSetted;

    private void Awake()
    {
        _player = GetComponent<Player>();
        _wallet = _player.Wallet;
        _skins = CreateSkins();
    }

    private void OnEnable()
    {
        _wallet.RedCristallChanged += OnRedCristallChanged;
    }

    private void OnDisable()
    {
        _wallet.RedCristallChanged -= OnRedCristallChanged;
    }

    private void Start()
    {
        if (Data.IsSeted == true)
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
        SetSavedSkinsStatus();
        CurrentSkin = _skins.FirstOrDefault(skin => skin.IsSelected == true);

        SkinsSetted?.Invoke();
        if (CurrentSkin != null)
            SelectSkin(CurrentSkin);
        if (CurrentSkin == null)
        {            
            PurchaseSkin(_skins[0]);            
        }
    }

    public void SelectSkin(Skin targetSkin)
    {
        CurrentSkin = targetSkin;
        foreach (var skin in _skins)
        {
            skin.SkinData.DeactivateAbilities(_player);
        }

        foreach (var skin in _skins)
        {
            if (skin.SkinData.Name == targetSkin.SkinData.Name)
            {
                skin.Select();
                _visualizer.ShowSkin(skin);
                skin.SkinData.ActivateAbilities(_player);
                continue;
            }
            skin.Unselect();
        }
        SaveSkins();
    }

    public void PurchaseSkin(Skin skin)
    {
        skin.Purchase(skin);
        _wallet.SpendRedCristals(skin.SkinData.Price);
        SelectSkin(skin);
    }

    private void SaveSkins()
    {
        SkinSavedData[] savedDatas = new SkinSavedData[_skins.Length];
        for (int i = 0; i < savedDatas.Length; i++)
        {
            savedDatas[i] = new SkinSavedData(_skins[i]);
        }
        _data.SetSkins(savedDatas);
    }

    private Skin[] CreateSkins()
    {
        Skin[] skins = new Skin[_skinsDatas.Length];
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i] = new Skin(_skinsDatas[i]);
        }
        return skins;
    }

    private void SetSavedSkinsStatus()
    {
        SkinSavedData[] savedDatas = _data.GetSkins();

        foreach (var skin in _skins)
        {
            foreach (var savedData in savedDatas)
            {
                if (skin.SkinData.Name == savedData.Name)
                {
                    skin.SetSavedData(savedData);
                }
            }
        }
    }
    private void OnRedCristallChanged(int redCount)
    {
        foreach (var skin in _skins)
        {
            skin.SetSaleStatus(redCount);
        }
    }
}
