using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Finder<T, I> : MonoBehaviour where T : IActivable
{
    private List<T> _collisionElements = new List<T>();
    private List<T> _activesElements => _collisionElements.Where(element => element.IsActive == true).ToList();

    public bool IsDetected => _activesElements.Count > 0;

    public event Action<I> Finded;
    public event Action Lossed;

    private void OnTriggerEnter(Collider other)
    {
        TryFind(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TryLose(other);
    }

    protected void TryFind(Collider other)
    {
        if (other.TryGetComponent(out T element))
        {
            if (element.IsActive == false)
                return;
            if (_collisionElements.Contains(element) == false)
                _collisionElements.Add(element);
            I target = GetTarget(element);
            Finded?.Invoke(target);
        }
    } 

    protected void TryLose(Collider other)
    {
        if (other.TryGetComponent(out T element))
        {
            Lose(element);
        }
    }

    protected void Lose(T element)
    {
        if (_collisionElements.Contains(element) == true)
            _collisionElements.Remove(element);

        _collisionElements = _activesElements;

        if (_collisionElements.Count == 0)
        {
            Lossed?.Invoke();
        }
    }

    protected abstract I GetTarget(T element);
}
