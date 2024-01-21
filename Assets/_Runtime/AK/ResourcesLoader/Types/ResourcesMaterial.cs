using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourcesMaterial : ResourcesAsset<Material>
{
    public void SetMaterial(Renderer renderer)
    {
        SetMaterial(renderer, 0);
    }

    public void SetMaterial(Renderer renderer, int index)
    {
        LoadAsset((data) => { renderer.sharedMaterials[index] = data; });
    }
}