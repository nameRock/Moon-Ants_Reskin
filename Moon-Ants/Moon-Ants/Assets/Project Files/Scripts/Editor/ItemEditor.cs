using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
[CanEditMultipleObjects]

public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Refresh elements"))
        {
            SerializedProperty elementsProperty = serializedObject.FindProperty("_elements");
            Item item = (Item)target;
            var elements = item.gameObject.GetComponentsInChildren<Element>();
            elementsProperty.ClearArray();
            for (int i = 0; i < elements.Length; i++)
            {
                elementsProperty.InsertArrayElementAtIndex(i);
                elementsProperty.GetArrayElementAtIndex(i).objectReferenceValue = elements[i];
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
