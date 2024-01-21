using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//
[CustomPropertyDrawer(typeof(ResourcesPrefab<GameObject>))]
public class ResourcesGameObjectPrefabDrawer : ResourcesAssetDrawer<GameObject> { }

//
[CustomPropertyDrawer(typeof(ResourcesPrefab<Transform>))]
public class ResourcesTransformPrefabDrawer : ResourcesAssetDrawer<Transform> { }

//
[CustomPropertyDrawer(typeof(ResourcesPrefab<Image>))]
public class ResourcesImagePrefabDrawer : ResourcesAssetDrawer<Image> { }

//
[CustomPropertyDrawer(typeof(ResourcesPrefab<RectTransform>))]
public class ResourcesRectTransformPrefabDrawer : ResourcesAssetDrawer<RectTransform> { }

//
[CustomPropertyDrawer(typeof(ResourcesPrefab<Text>))]
public class ResourcesRectTextPrefabDrawer : ResourcesAssetDrawer<Text> { }

