using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ResourcesAssetDrawer<T> : PropertyDrawer where T: UnityEngine.Object
{
    private const float textWidth = 0.8f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        UpdatePropertyAssetState(property);

        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var filePathProperty = property.FindPropertyRelative("filePath");
        var referenceProperty = property.FindPropertyRelative("fileGuid");
        var resourceFilePathProperty = property.FindPropertyRelative("resourceFilePath");

        var filePath        = AssetDatabase.GUIDToAssetPath(referenceProperty.stringValue);
        var loadedFile      = AssetDatabase.LoadAssetAtPath<T>(filePath);
        var referenceObject = EditorGUI.ObjectField(new Rect(position.x, position.y, position.width * (1f - textWidth), position.height), loadedFile, typeof(T), false);

        if (GUI.changed && referenceObject != null)
        {
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(referenceObject, out string guid, out long _))
            {
                var selectedFilePath = AssetDatabase.GUIDToAssetPath(guid);

                property.FindPropertyRelative("fileName").stringValue = referenceObject.name;

                referenceProperty.stringValue = guid;
                referenceProperty.serializedObject.ApplyModifiedProperties();

                filePathProperty.stringValue = selectedFilePath;
                filePathProperty.serializedObject.ApplyModifiedProperties();

                var resourcePath = ResourcesAssetExt.GetResourceFilePath(selectedFilePath);
                resourceFilePathProperty.stringValue = resourcePath;
                resourceFilePathProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        GUI.enabled = false;
        EditorGUI.TextArea(new Rect(position.x + (position.width * (1f - textWidth)), position.y, position.width * textWidth, position.height), resourceFilePathProperty.stringValue);
        GUI.enabled = true;
        EditorGUI.EndProperty();
    }

    private void UpdatePropertyAssetState(SerializedProperty property)
    {
        //var filePathProperty = property.FindPropertyRelative("filePath");
        //var referenceProperty = property.FindPropertyRelative("fileGuid");
        //var resourceFilePathProperty = property.FindPropertyRelative("resourceFilePath");
    }

}
