using UnityEngine;

public class SkinElement : MonoBehaviour
{
    [SerializeField] private SkinData _skinData;

    protected bool IsActive { get; private set; } = false;

    public virtual void TryAcivate(Skin skin)
    {   
        SetState(skin);
    }

    protected void SetState(Skin skin)
    {
        IsActive = _skinData == skin.SkinData;
        gameObject.SetActive(IsActive);
    }
}
