using System.Linq;
using UnityEngine;

public class ChildsSortier : MonoBehaviour
{
    [ContextMenu("RenameAndSort")]
    public void RenameAndSort()
    {
        Transform[] childs = new Transform[transform.childCount];
        for (int i = 0; i < childs.Length; i++)
            childs[i] = transform.GetChild(i);

        var sortChilds = childs.OrderBy(p => p.position.y).ToArray();
   
        for (int i = 0; i < transform.childCount; i++)
        {
            sortChilds[i].name = i.ToString();
        }
        Sort();
    }

    [ContextMenu("Sort")]
    public void Sort()
    {
        int maxCount = transform.childCount;

        Transform[] childs = new Transform[transform.childCount];
        for (int i = 0; i < childs.Length; i++) 
            childs[i] = transform.GetChild(i);

        var sortChilds = childs.OrderBy(p => int.Parse(p.gameObject.name)).ToArray();
        for (int i = 0; i < transform.childCount; i++)
        {
            sortChilds[i].SetSiblingIndex(i);
        }
    }
}
