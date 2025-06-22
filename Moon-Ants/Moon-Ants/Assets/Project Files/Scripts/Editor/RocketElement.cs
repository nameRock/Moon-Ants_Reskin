using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RocketBuilder))]
[CanEditMultipleObjects]

public class RocketEditor : Editor
{
    Transform root;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        root = EditorGUILayout.ObjectField("root", root, typeof(Transform), true) as Transform;
        if (GUILayout.Button("Refresh elements"))
        {
            SerializedProperty elementsProperty = serializedObject.FindProperty("_elements");
            var elements = root.GetComponentsInChildren<RocketElement>(true);
            elementsProperty.ClearArray();
            for (int i = 0; i < elements.Length; i++)
            {
                elementsProperty.InsertArrayElementAtIndex(i);
                elementsProperty.GetArrayElementAtIndex(i).objectReferenceValue = elements[i].gameObject;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
