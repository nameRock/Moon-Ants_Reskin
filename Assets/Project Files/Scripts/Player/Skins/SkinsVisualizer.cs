using UnityEngine;

public class SkinsVisualizer : MonoBehaviour
{
    private SkinElement[] _elements;

    private void Awake()
    {
        _elements = GetComponentsInChildren<SkinElement>(true);
    }

    public void ShowSkin(Skin skin)
    {        
        foreach (var element in _elements)
        {
            element.TryAcivate(skin);
        }
    }
}
