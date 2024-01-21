using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class ResourcesAssetExt
{
#if UNITY_EDITOR
    public static void SetAsset<T>(this ResourcesAsset<T> asset, T referenceObject) where T : UnityEngine.Object
    {
        if (referenceObject == null) return;

        if (UnityEditor.AssetDatabase.TryGetGUIDAndLocalFileIdentifier(referenceObject, out string guid, out long _))
        {
            var selectedFilePath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

            asset.FileName     = referenceObject.name;
            asset.FileGuid     = guid;
            asset.FullFilePath = selectedFilePath;

            var resourcePath = GetResourceFilePath(selectedFilePath);
            asset.ResourceFilePath = resourcePath;
        }
    }
#endif

    public static string GetResourceFilePath(string path)
    {
        if (!path.Contains("/Resources/"))
        {
            Debug.LogError("File is not in Resources Folder");
            return string.Empty;
        }

        int           foundCount            = 0;
        bool          canAddPath            = false;
        StringBuilder resourceFolderBuilder = new StringBuilder("/Resources/");
        StringBuilder pathBuilder           = new StringBuilder(path);
        StringBuilder resultPathBuilder     = new StringBuilder();

        for (int index = 0; index < pathBuilder.Length; index++)
        {
            var currentChar = pathBuilder[index];
            if (canAddPath)
            {
                if (currentChar == '.') break;

                resultPathBuilder.Append(currentChar);
                continue;
            }

            if (currentChar == resourceFolderBuilder[foundCount])
            {
                foundCount++;
                canAddPath = (foundCount >= resourceFolderBuilder.Length);
            }
            else if (foundCount > 0)
            {
                foundCount = 0;
            }
        }

        return resultPathBuilder.ToString();
    }
}