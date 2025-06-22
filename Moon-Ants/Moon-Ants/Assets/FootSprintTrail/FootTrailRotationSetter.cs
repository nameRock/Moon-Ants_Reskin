using UnityEngine;

public class FootTrailRotationSetter : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _footTrailsEffects;

    private void Update()
    {
        foreach (var footTrail in _footTrailsEffects)
        {
            var main = footTrail.main;
            main.startRotation = transform.rotation.eulerAngles.y /Mathf.Rad2Deg;
        }
    }
}
