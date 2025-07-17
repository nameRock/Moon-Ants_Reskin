using UnityEngine;

public class EnemyTargetSelector : MonoBehaviour
{
    [SerializeField] private EnemiesBaza _enemiesBaza;
    [SerializeField] private PlayerBaza _playerBaza;
    [SerializeField, Range(0, 1f)] private float _aggression = 0.5f;

    public TargetType TargetType { get; private set; }
    public Transform Target { get; private set; }

    private void Start()
    {
        SetNewTarget(out Transform target);
    }

    public void SetNewTarget(out Transform target)
    {
        float currentAgression = GetRandomValue();
        if (currentAgression < _aggression)
            Target = GetAntTarget();
        else
            Target = GetItemTarget();
        target = Target;
    }

    public void SetBazaAsTarget(out Transform target)
    {
        Target = _enemiesBaza.transform;
        TargetType = TargetType.Baza;
        target = Target;
    }

    private Transform GetAntTarget()
    {
        TargetType = TargetType.Ant;
        if (_playerBaza.AntMionMovers.Count == 0)
        {
            return null;
        }
        int index = Random.Range(0, _playerBaza.AntMionMovers.Count);       
        return _playerBaza.AntMionMovers[index].transform;
    }

    private Transform GetItemTarget()
    {
        TargetType = TargetType.Item;
        Element element = _enemiesBaza.GetCurrentItem().GetNextElement();       
        if (element == null)
            return null;
        return element.transform;
    }

    private float GetRandomValue()
    {
        float maxValue = 1f;
        return Random.Range(0, maxValue);
    }
}
public enum TargetType
{
    Ant,
    Item,
    Baza
}
