using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SO_Item))]
public class SO_ItemEditor : Editor
{
    SerializedProperty dataProperty;
    SerializedProperty isStackableProperty;
    SerializedProperty maxQuantityAllowedProperty;

    void OnEnable()
    {
        dataProperty = serializedObject.FindProperty("data");
        isStackableProperty = dataProperty.FindPropertyRelative("isStackable");
        maxQuantityAllowedProperty = dataProperty.FindPropertyRelative("maxQuantityAllowed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("itemID"));
        EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("itemName"));
        EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("icon"));
        EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("description"));
        EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("itemType"));
        EditorGUILayout.PropertyField(isStackableProperty);

        if (isStackableProperty.boolValue)
        {
            EditorGUILayout.PropertyField(maxQuantityAllowedProperty);
        }

        EditorGUILayout.PropertyField(dataProperty.FindPropertyRelative("price"));

        serializedObject.ApplyModifiedProperties();
    }
}