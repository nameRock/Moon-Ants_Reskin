using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowMultiple : MonoBehaviour
{
    public event Action <int> Multiplied;

    public void MultipliRewarded(int multipli)
    {
        Multiplied?.Invoke(multipli);
    }
}
