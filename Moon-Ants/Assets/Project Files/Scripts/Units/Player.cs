using System;
using UnityEngine;

[RequireComponent(typeof(PlayerCollector), typeof(PlayerMover))]
public class Player : Unit
{
    [SerializeField] private PlayerAttacker _playerAttacker;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private ParticleSystem _upgradeBuff;

    private PlayerCollector _playerCollector;
    private PlayerMover _playerMover;


    public Wallet Wallet => _wallet;
    public PlayerCollector PlayerCollector => _playerCollector;
    public PlayerMover PlayerMover => _playerMover;
    public PlayerAttacker PlayerAttacker => _playerAttacker;

    private void OnEnable()
    {
        WebSdk.ADPlayed += OnAdPlayed;
    }

    private void OnDisable()
    {
        WebSdk.ADPlayed -= OnAdPlayed;
    }

    private void Awake()
    {
        _playerCollector = GetComponent<PlayerCollector>();
        _playerMover = GetComponent<PlayerMover>();
    }

    public override void TryTakeDamage(AttackData attackData)
    {
        return;
    }

    public void DeactivateForWhile(float delay)
    {

    }


    protected override void Kill()
    {
        return;
    }

    public void PlayUpgradeEffect()
    {
        _upgradeBuff.Play();
    }

    private void OnAdPlayed(bool isADPlayer)
    {
        _playerMover.SetState(isADPlayer == false);
    }
}
