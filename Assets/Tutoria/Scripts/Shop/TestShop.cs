using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Linq;
using UnityEngine.UI;

public class TestShop : MonoBehaviour, IStoreListener
{
    [SerializeField] private UIProduct _uiProductPrefab;
    [SerializeField] private HorizontalLayoutGroup _contentPanel;
    [SerializeField] private GameObject _loadingOverlay;

    private IStoreController _storeController;
    private IExtensionProvider _extensionProvider;
    private Action OnPurchaseCompleted;

    private async void Awake()
    {
        InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            .SetEnvironmentName("test");
#else
            .SetEnvironmentName("production");
#endif
        await UnityServices.InitializeAsync(options);
        ResourceRequest operation = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
        operation.completed += HandleIAPCatalogLoader;
    }

    private void HandleIAPCatalogLoader(AsyncOperation operation)
    {
        ResourceRequest request = operation as ResourceRequest;

        Debug.Log($"Loaded Asset: {request.asset}");
        ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
        Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

        StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
        StandardPurchasingModule.Instance().useFakeStoreAlways = true;

#if UNITY_ANDROID
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));
#elif UNITY_IOS
ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.AppleAppStore));
#else
ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.NotSpecified));
#endif
        foreach (ProductCatalogItem item in catalog.allProducts)
        {
            builder.AddProduct(item.id, item.type);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        _storeController = controller;
        _extensionProvider = extensions;
        StoreIconManager.Initialize(_storeController.products);
        StoreIconManager.OnLoadCompleted += HandleAllIconsLoaded;
    }

    private void HandleAllIconsLoaded()
    {
        StartCoroutine(CreateUI());
    }

    private IEnumerator CreateUI()
    {
        List<Product> sortedProducts = _storeController.products.all.OrderBy(item => item.metadata.localizedPrice).ToList();

        foreach (Product product in sortedProducts)
        {
            UIProduct uIProduct = Instantiate(_uiProductPrefab);
            uIProduct.OnPurchase += HandlePurchase;
            uIProduct.Setup(product);
            uIProduct.transform.SetParent(_contentPanel.transform, false);
            yield return null;
        }

        HorizontalLayoutGroup group = _contentPanel.GetComponent<HorizontalLayoutGroup>();
        float spacing = group.spacing;
        float horizontalPadding = group.padding.left + group.padding.right;
        float itemSize = _contentPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;

        RectTransform rect = _contentPanel.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(horizontalPadding + (spacing + itemSize) * sortedProducts.Count, rect.sizeDelta.y);
    }

    private void HandlePurchase(Product model, Action OnComplete)
    {
        _loadingOverlay.SetActive(true);
        OnPurchaseCompleted = OnComplete;
        _storeController.InitiatePurchase(model);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Error Initializing IAP because of {error}." + $"\r\nShow a message to the player depending on the error");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Faield to purchase {product.definition.id} because {failureReason}");
        OnPurchaseCompleted?.Invoke();
        OnPurchaseCompleted = null;
        _loadingOverlay.SetActive(false);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        Debug.Log($"Successfully to purchase {purchaseEvent.purchasedProduct.definition.id}");
        OnPurchaseCompleted?.Invoke();
        OnPurchaseCompleted = null;
        _loadingOverlay.SetActive(false);

        //aqui precisa de um sistema q faça o jogador receber os recursos que comprou no jogo
        if (Enum.TryParse(purchaseEvent.purchasedProduct.definition.id.ToUpper(), out ShopResourcesManager.ShopItemTypes shopItemType))
        {
            ShopResourcesManager.BuyProduct(shopItemType, purchaseEvent.purchasedProduct);
        }
        else
        {
            Debug.LogError($"faield to apply value to {purchaseEvent.purchasedProduct.metadata.localizedTitle}");
        }


        return PurchaseProcessingResult.Complete;
    }
}
