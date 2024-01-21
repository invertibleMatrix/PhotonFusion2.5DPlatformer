using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ResourcesTexture))]
public class ResourcesTextureDrawer : ResourcesAssetDrawer<Texture>
{
}
