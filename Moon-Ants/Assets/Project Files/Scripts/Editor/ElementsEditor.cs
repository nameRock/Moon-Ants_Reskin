using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Element))]
[CanEditMultipleObjects]
public class ElementsEditor : Editor
{
    SerializedProperty collider;

    void OnEnable()
    {
        collider = serializedObject.FindProperty("_meshCollider");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Update"))
        {
            foreach (var go in Selection.gameObjects)
            {
                if (go.TryGetComponent<Element>(out Element element))
                {
                    Selection.activeObject = element;
                    SerializedObject serializedObject = new SerializedObject(element);
                    SerializedProperty property = serializedObject.FindProperty("_meshCollider");
                    property.objectReferenceValue = element.GetComponent<MeshCollider>();
                    serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
