using UnityEngine;

public class AlienScaler : TargetBuilder
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _maxCount;

    public override void StartActivateElement()
    {
        GetData();
        SetScale(CurrentIndexElement, _maxCount);
        CheckCompletion();
    }

    protected override void Builde(string character = "Ant")
    {
        PlayAnimation(character);
        ScaleElement();
        SetData();
    }

    protected override int GetMaxElementCount()
    {
        return _maxCount;
    }

    private void ScaleElement()
    {
        for (int i = 0; i < GetMultiplierElement(); i++)
        {
            CurrentIndexElement++;
        }
        SetScale(CurrentIndexElement, _maxCount);
    }

    private void SetScale(int currentIndexElement, int length)
    {
        _meshRenderer.SetBlendShapeWeight(0, (float)currentIndexElement / length * 100);
    }

    private void PlayAnimation(string character)
    {
        if (character == "Ant")
            _animator.SetTrigger(AnimatorConst.Eat);
    }
}