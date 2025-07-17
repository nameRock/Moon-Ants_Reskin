using System;
using UnityEngine;

public class AnimatorCallbacker : MonoBehaviour
{
    public event Action HitCalled;

    // //Call from Animator
    private void TryHitTarget()
    {
        HitCalled?.Invoke();
    }
}
