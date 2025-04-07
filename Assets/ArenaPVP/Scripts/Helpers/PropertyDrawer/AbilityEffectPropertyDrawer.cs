#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[CustomPropertyDrawer(typeof(AbilityEffectBase), true)]
public class AbilityEffectPropertyDrawer : PropertyDrawer
{
    private bool isFoldout;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the foldout with the clean type name or a default label if not selected
        string foldoutLabel = GetCleanTypeName(property.managedReferenceFullTypename) ?? "Select Type";
        isFoldout = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), isFoldout, foldoutLabel);

        // Calculate the new position for the button
        position.y += EditorGUIUtility.singleLineHeight;

        // Draw the button on the same line as the foldout if no type is selected
        if (property.managedReferenceValue == null)
        {
            Rect buttonRect = new Rect(position.x + 100, position.y - EditorGUIUtility.singleLineHeight, position.width - 100, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(buttonRect, "Choose Type"))
            {
                ShowTypeSelectionMenu(property);
            }
        }
        // Draw the button and properties below the foldout if a type is selected and the foldout is expanded
        else if (isFoldout)
        {
            Rect buttonRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            if (GUI.Button(buttonRect, "Choose Type"))
            {
                ShowTypeSelectionMenu(property);
            }

            // Adjust position for the properties to avoid overlap with the button
            position.y += EditorGUIUtility.singleLineHeight;

            // Indent the child properties
            EditorGUI.indentLevel++;

            // Draw the properties of the selected derived class
            SerializedProperty currentProperty = property.Copy();
            var depth = currentProperty.depth;
            while (currentProperty.NextVisible(true) && currentProperty.depth > depth)
            {
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), currentProperty, true);
                position.y += EditorGUIUtility.singleLineHeight; // Move to the next line for the next property
            }

            // Reset indent level
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    private void ShowTypeSelectionMenu(SerializedProperty property)
    {
        GenericMenu menu = new GenericMenu();
        foreach (var type in GetSubclassesOf<AbilityEffectBase>())
        {
            menu.AddItem(new GUIContent(type.Name.Split('.').Last()), false, () =>
            {
                property.managedReferenceValue = Activator.CreateInstance(type);
                property.serializedObject.ApplyModifiedProperties();
            });
        }
        menu.ShowAsContext();
    }

    private string GetCleanTypeName(string fullTypeName)
    {
        if (string.IsNullOrEmpty(fullTypeName))
            return "None";

        // Split by space to remove assembly name
        return fullTypeName.Split(' ').Last();
    }

    private List<Type> GetSubclassesOf<T>()
    {
        var type = typeof(T);
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract)
            .ToList();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Always include the height for the foldout
        int fieldCount = 1;

        // Add height for the button if no type is selected or foldout is expanded
        if (property.managedReferenceValue == null || isFoldout)
        {
            fieldCount++;
        }

        // Add height for each property if the foldout is expanded and a type is selected
        if (isFoldout && property.managedReferenceValue != null)
        {
            SerializedProperty currentProperty = property.Copy();
            var depth = currentProperty.depth;
            while (currentProperty.NextVisible(true) && currentProperty.depth > depth)
            {
                fieldCount++;
            }
        }

        // If no effect is selected and foldout is not expanded, only show the foldout
        if (property.managedReferenceValue == null && !isFoldout)
        {
            fieldCount = 1;
        }

        return fieldCount * EditorGUIUtility.singleLineHeight;
    }
}
#endif