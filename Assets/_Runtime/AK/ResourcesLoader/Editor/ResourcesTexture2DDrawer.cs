using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(ResourcesTexture2D))]
public class ResourcesTexture2DDrawer : ResourcesAssetDrawer<Texture2D>
{
}
