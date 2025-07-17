using UnityEngine;

public class IncreaseBase : MonoBehaviour
{
    [SerializeField] private Transform _transform;

    private float _speed = 2f;
    private Vector3 _targetScale = new Vector3(1.4f, 1.4f, 1f);
    private Vector3 _startScale;
    private bool _isPlayer = false;

    private void Start()
    {
        _startScale = _transform.localScale;
    }

    private void Update()
    {
        if (_isPlayer == true)
            Scale(_transform.localScale, _targetScale);
        else
            Scale(_transform.localScale, _startScale);
    }

    private void Scale(Vector3 startScale, Vector3 targetScale)
    {
        _transform.localScale = Vector3.MoveTowards(startScale, targetScale, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            _isPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player _))
        {
            _isPlayer = false;
        }
    }

}
