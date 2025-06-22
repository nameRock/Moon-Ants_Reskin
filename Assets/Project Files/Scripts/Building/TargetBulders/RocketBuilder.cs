using UnityEngine;

public class RocketBuilder : TargetBuilder
{
    [SerializeField] private GameObject[] _elements;

    protected override void Builde(string character = "Ant")
    {
        ActivateElement();
        SetData();
    }

    private void ActivateElement()
    {
        for (int i = 0; i < GetMultiplierElement(); i++)
        {

            if (CurrentIndexElement < _elements.Length && _elements[CurrentIndexElement] != null)
            {
                _elements[CurrentIndexElement].SetActive(true);
                CurrentIndexElement++;
            }
        }
    }

    public override void StartActivateElement()
    {
        GetData();
        {
            for (int i = 0; i < CurrentIndexElement; i++)
            {
                _elements[i].SetActive(true);
            }
        }
        CheckCompletion();
    }

    protected override int GetMaxElementCount()
    {
        return _elements.Length;
    }
}
