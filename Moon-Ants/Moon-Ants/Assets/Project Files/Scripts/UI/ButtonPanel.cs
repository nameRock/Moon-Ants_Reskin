using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPanel : MonoBehaviour
{
    private const string Open = "Open";
    private const string Close = "Close";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OpenPanel()
    {
        _animator.SetTrigger(Open);
    }

    public void ClosePanel()
    {
        _animator.SetTrigger(Close);
    }
}
