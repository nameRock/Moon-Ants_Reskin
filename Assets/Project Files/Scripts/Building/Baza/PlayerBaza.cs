using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AntParametersSetter))]
public class PlayerBaza : Baza
{
    //[SerializeField] private ButtonPanel _buttonPanel;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private float _valueRocket;

    private Coroutine _waitStartDrop;

    private void Start()
    {
#if UNITY_EDITOR
        LoadSpawnAnts();
        return;
#endif

        if (Data.IsSeted == true)
        {
            LoadSpawnAnts();
            return;
        }
        Data.Setted += OnDataSeted; 
    }

    private void OnDataSeted()
    {
        LoadSpawnAnts();
        Data.Setted -= OnDataSeted;
    }

    public override void AddValueWallet(Element element, string character)
    {
        if (element.IsEnerge)
        {
            if (element.IsGreen)
            {
                _wallet.AddGreenCristals(1, "Mining");
            }
            else
            {
                _wallet.AddBlueCristals(element.Price, "Mining", character);
            }
        }
        else
        {
            _targetBuilder.AddElement(_valueRocket, character);
        }
    }

    public void ChagedMultiplier(int lvl)
    {
        _wallet.ChangeMultiplier(lvl);
    }

    protected override void StartDrop(PlayerCollector collector)
    {
        if (collector.TryGetComponent(out PlayerMover playerMover) == false)
            return;

        collector.Drop(transform, true);
    }

    protected override void EndDrop(PlayerCollector collector)
    {
        collector.StopDrop();
    }   
}
