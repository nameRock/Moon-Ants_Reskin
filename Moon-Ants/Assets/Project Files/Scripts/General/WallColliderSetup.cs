using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColliderSetup : MonoBehaviour
{
    private void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider collider = GetComponent<MeshCollider>();

        collider.sharedMesh = meshFilter.mesh;
    }
}
