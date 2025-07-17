using System.Linq;
using UnityEngine;

public class MeshScaler : MonoBehaviour
{
    [SerializeField] private Vector3 _maxScale = new Vector3(0.25f, 0.25f, 0.25f);


    [ContextMenu("SetTargetsScales")]
    public void SetTargetsScales()
    {
        var elements = GetComponentsInChildren<Element>();
        foreach (var element in elements)
        {
            SetScale(element);
        }
    }

    private void SetScale(Element element)
    {
        MeshFilter meshFilter = element.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.sharedMesh;

        var vertices = mesh.vertices;
        var xSortierVertex = vertices.OrderBy(v => v.x).ToArray();
        var ySortierVertex = vertices.OrderBy(v => v.y).ToArray();
        var zSortierVertex = vertices.OrderBy(v => v.z).ToArray();

        float xScale = GetMaxPoint(xSortierVertex).x - GetMinPoint(xSortierVertex).x;
        float yScale = GetMaxPoint(ySortierVertex).y - GetMinPoint(ySortierVertex).y;
        float zScale = GetMaxPoint(zSortierVertex).z - GetMinPoint(zSortierVertex).z;

        float targetXScale = _maxScale.x / xScale;
        float targetYScale = _maxScale.y / yScale;
        float targetZScale = _maxScale.z / zScale;
    }

    private Vector3 GetMaxPoint(Vector3[] verticles) => verticles[verticles.Length - 1];
    private Vector3 GetMinPoint(Vector3[] verticles) => verticles[0];
}