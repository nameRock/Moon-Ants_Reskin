using UnityEngine;

[RequireComponent(typeof(AntMionMover))]
public class AssistantEnemy : Enemy
{
    [SerializeField] private Item _itemPrefab;
    [SerializeField] private Vector3 _itemPositionOffset = new Vector3(0, 0.5f, 0);

    private AntMionMover _mionMover;
    public AntMionMover AntMionMover => _mionMover;

    private void Awake()
    {
        _mionMover = GetComponent<AntMionMover>();
    }

    protected override void Kill()
    {        
        Item item = Instantiate(_itemPrefab, transform.position - _itemPositionOffset, Quaternion.Euler(new Vector3(90, 0, 0)));
        item.GetComponent<Element>().EnableCollider();
        _mionMover.Kill();
    }
}
