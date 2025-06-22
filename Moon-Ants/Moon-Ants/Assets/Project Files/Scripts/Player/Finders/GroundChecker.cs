using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private float _checkingRadius = 3f;
    [SerializeField] private Transform _mainChekingCenter;
    [SerializeField] private Transform _rightChekingCenter;
    [SerializeField] private Transform _leftChekingCenter;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _maxCorrectedAngle = 60f;

    public bool TryCheckTarget(out float correctedAngle)
    {
        correctedAngle = 0;
        if (IsGrounded(_mainChekingCenter.position) == true)
            return true;
        if (IsGrounded(_rightChekingCenter.position) == true)
        {
            correctedAngle = _maxCorrectedAngle;
            return true;
        }
        if (IsGrounded(_leftChekingCenter.position) == true)
        {
            correctedAngle = -_maxCorrectedAngle;
            return true;
        }
        return false;
    }

    private bool IsGrounded(Vector3 center)
    {
        return Physics.Raycast(center, Vector3.down, _checkingRadius, _layerMask);
    }
    private void OnDrawGizmos()
    {
        if (_mainChekingCenter == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_mainChekingCenter.position, _mainChekingCenter.position - new Vector3(0, _checkingRadius, 0));

    }
}
