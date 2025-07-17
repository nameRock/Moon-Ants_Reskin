using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipliElement : MonoBehaviour
{
    [SerializeField] private int _multipli;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out ArrowMultiple arrowMultiple))
        {
            arrowMultiple.MultipliRewarded(_multipli);
        }
    }
}
