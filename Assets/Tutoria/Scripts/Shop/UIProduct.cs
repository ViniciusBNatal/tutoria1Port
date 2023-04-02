using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using System;

public class UIProduct : MonoBehaviour
{
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Text _priceText;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _purchaseBtn;

    private Product _model;

    public delegate void PurchaseEvent(Product model, Action OnComplete);
    public event PurchaseEvent OnPurchase;

    public void Setup(Product product)
    {
        _model = product;
        _nameText.text = product.metadata.localizedTitle;
        _descriptionText.text = product.metadata.localizedDescription;
        _priceText.text = $"{product.metadata.localizedPriceString}" + $"{product.metadata.isoCurrencyCode}";

        Texture2D texture = StoreIconManager.GetTexture(product.definition.id);
        if (texture != null)
        {
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one / 2f);
            _icon.sprite = sprite;
        }
        else
        {
            Debug.LogError($"No sprite found for {product.definition.id}");
        }
    }

    public void Purchase()
    {
        _purchaseBtn.enabled = false;
        OnPurchase?.Invoke(_model, HandlePurchaseCompleted);
    }

    private void HandlePurchaseCompleted()
    {
        _purchaseBtn.enabled = true;
    }
}
