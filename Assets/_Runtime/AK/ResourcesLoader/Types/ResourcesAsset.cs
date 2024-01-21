using System;
using UnityEngine;

[Serializable]
public abstract class ResourcesAsset<T> where T : UnityEngine.Object
{
// #pragma warning disable IDE0052 // Remove unread private members
    [SerializeField] private string fileName         = string.Empty;
    [SerializeField] private string fileGuid         = string.Empty;
    [SerializeField] private string resourceFilePath = string.Empty;
    [SerializeField] private string filePath         = string.Empty;
// #pragma warning restore IDE0052 // Remove unread private members

    public string FileName
    {
        get { return fileName; }
        set { fileName = value; }
    }

    public string FileGuid
    {
        get { return fileGuid; }
        set { fileGuid = value; }
    }

    public string FullFilePath
    {
        get { return filePath; }
        set { filePath = value; }
    }

    public string ResourceFilePath
    {
        get { return resourceFilePath; }
        set { resourceFilePath = value; }
    }

    public T    LoadedFile    { get; set; }
    public bool IsLoadingFile { get; private set; }

    private delegate void AssetLoadingEvent(T asset);

    private AssetLoadingEvent OnAssetLoadedCallback;

    public virtual void LoadAsset(Action<T> loadCallback)
    {
        LoadAsset(ResourceFilePath, loadCallback);
    }

    public virtual void LoadAsset(string resourcesPath, Action<T> loadCallback)
    {
        if (LoadedFile != null)
        {
            loadCallback.Invoke(LoadedFile);
            return;
        }

        if (IsLoadingFile)
        {
            OnAssetLoadedCallback += (asset) => loadCallback?.Invoke(asset);
            return;
        }

        IsLoadingFile = true;
        ResourceRequest request = Resources.LoadAsync<T>(resourcesPath);

        request.completed += (progress) =>
        {
            LoadedFile = (T)request.asset;
            loadCallback.Invoke(LoadedFile);
            OnAssetLoadedCallback?.Invoke(LoadedFile);
        };
    }
}