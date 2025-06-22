using System.Collections;
using UnityEngine;

public class EnemiesBaza : Baza
{
    [SerializeField] private float _spawnDelay = 1f;
    [SerializeField] private bool _isSpawned = true;

    private void Start()
    {
        if (_isSpawned == true)
            StartCoroutine(WaitSpawnNextAnt());
    }

    protected override void EndDrop(PlayerCollector collector)
    {
        collector.StopDrop();
    }

    protected override void StartDrop(PlayerCollector collector)
    {
        collector.Drop(transform, true);
    }

    private IEnumerator WaitSpawnNextAnt()
    {
        while (true)
        {
            SpawnAnt(_antPrefab);
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    public override void AddValueWallet(Element element, string ñharacter)
    {
    }
}
