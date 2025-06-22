using System.Collections;
using UnityEngine;

public class OpenElement : MonoBehaviour
{
    [SerializeField] private OxygenZone _oxygenZone;
    [SerializeField] private Baza _baza;
    [SerializeField] private Item[] _items;
    [SerializeField] private SizeChangedAnimation _grassAnimation;

    private void OnEnable()
    {
        _oxygenZone.Opened += OnZoneOpened;
    }

    private void OnDisable()
    {
        _oxygenZone.Opened -= OnZoneOpened;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_oxygenZone.IsFullOpened == true)
            return;

        if (other.gameObject.TryGetComponent(out Player player))
        {
            TryOpenZone(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player wallet))
        {
            _oxygenZone.TryStopOpening();
        }
    }

    private void TryOpenZone(Player player)
    {

        if (player.Wallet.BlueCountValue > 0 || player.PlayerCollector.ContainsEnerge == true)
        {
            _oxygenZone.StartOpening(player);  
        }
    }   

    private void OnZoneOpened()
    {
        TryPlayEnvironmentAnimation();
        DisableObject();
    }

    private void TryPlayEnvironmentAnimation()
    {
        if (_grassAnimation == null)
            return;
        _grassAnimation.gameObject.SetActive(true);
        _grassAnimation.Play();
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);

        for (int i = 0; i < _items.Length; i++)
        {
            _baza.AddItem(_items[i]);
        }

        _baza.RestartMoveAnts();
    }
}
