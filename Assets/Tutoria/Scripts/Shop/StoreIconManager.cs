using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class StoreIconManager
{
    public static Dictionary<string, Texture2D> Icons { get; private set; } = new Dictionary<string, Texture2D>();
    private static int TargetIconCount;
    public delegate void LoadCompleteAction();
    public static event LoadCompleteAction OnLoadCompleted;

    public static void Initialize(ProductCollection collection)
    {
        if (Icons.Count == 0)
        {
            Debug.Log($"Loading store icons for {collection.all.Length} products");
            TargetIconCount = collection.all.Length;
            //para isso funcionar o nome da textura deve ser IGUAL ao ID q foi colocado no IAP Catalog
            foreach(Product product in collection.all)
            {
                Debug.Log($"Loading store icons at path Resources/ShopIcons/{product.definition.id}");
                ResourceRequest operation = Resources.LoadAsync<Texture2D>($"ShopIcons/{ product.definition.id}");
                operation.completed += HandleLoadIcon;
            }
        }
        else
        {
            Debug.LogWarning("StoreIconManager Already initialized");
        }
    }

    public static Texture2D GetTexture(string id)
    {
        if(Icons.Count == 0)
        {
            Debug.LogError("Called StoreIconManager before initializing" + "This operation is not supported");
            throw new InvalidOperationException("StoreIconManager.GetTexture() cannot be called before calling" + "StoreIconManager.Initialize()");
        }
        else
        {
            Icons.TryGetValue(id, out Texture2D texture);
            return texture;
        }
    }

    private static void HandleLoadIcon(AsyncOperation operation)
    {
        ResourceRequest request = operation as ResourceRequest;
        if(request.asset != null)
        {
            Debug.Log($"Successfully loaded {request.asset.name}");
            Icons.Add(request.asset.name, request.asset as Texture2D);

            if(Icons.Count == TargetIconCount)
            {
                OnLoadCompleted?.Invoke();
            }
        }
        else
        {
            // something went wrong with the load
            TargetIconCount--;
        }
    }
}
