using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _itemIconUI;
    [SerializeField] private ZoomInOut _zoomInOut;
    [Header("CollectItemAnimationVariables")]
    [SerializeField] private float _animationDuration;
    [SerializeField, Tooltip("Multiplicative")] private float _strechSize;
    [SerializeField] private Color _alreadyWithItemColor;
    [SerializeField] private Sprite _inventoryEmptyIcon;
    private string _itemID;
    private float _currentDelta;
    private Vector2 _initialImageSize;
    private Color _initialImageColor;
    private bool _isAnimating = false;
    public string CurrentItemID => _itemID;
    //[SerializeField] private Text _itemAmount;
    private void Awake()
    {
        _initialImageSize = _itemIconUI.GetComponent<RectTransform>().localScale;
        _initialImageColor = _itemIconUI.color;
    }
    public void UpdateInventoryIcon(CollectableItem collectableItem)
    {
        if (collectableItem)
        {
            if (string.IsNullOrEmpty(_itemID))
            {
                _itemIconUI.enabled = true;
                _itemIconUI.sprite = collectableItem.Data.ItemIcon;
                _itemID = collectableItem.Data.ItemID;
            }            
            TriggerAnimation(collectableItem.Data.ItemID);            
        }
        //remove o item do inventario
        else
        {
            _itemIconUI.enabled = false;
            _itemID = null;
        }
    }
    public void TriggerAnimation(string itemID)
    {
        if (string.IsNullOrEmpty(_itemID))
        {
            _itemIconUI.sprite = _inventoryEmptyIcon;
            _itemIconUI.enabled = true;
        }
        else if (itemID != _itemID)
        {            
            _itemIconUI.color = _alreadyWithItemColor;
        }
        _currentDelta = 0;
        if (!_isAnimating) StartCoroutine(StrechAnim());
    }
    IEnumerator StrechAnim()
    {
        _isAnimating = true;
        bool isGrowing = true;
        WaitForFixedUpdate delay = new WaitForFixedUpdate();
        RectTransform imageTransform = _itemIconUI.GetComponent<RectTransform>();
        while (_isAnimating)
        {
            if (isGrowing)
            {
                Vector2 newSize = Vector2.Lerp(_initialImageSize, _initialImageSize * _strechSize, _currentDelta);
                imageTransform.localScale = newSize;
                if (_currentDelta >= 1f)
                {
                    isGrowing = false;
                    _currentDelta = 0;
                }
            }
            else
            {
                Vector2 newSize = Vector2.Lerp(_initialImageSize * _strechSize, _initialImageSize, _currentDelta);
                imageTransform.localScale = newSize;
                if (_currentDelta >= 1f)
                {
                    _itemIconUI.color = _initialImageColor;
                    if(_itemIconUI.sprite == _inventoryEmptyIcon) _itemIconUI.enabled = false;
                    _isAnimating = false;
                    yield break;
                }
            }
            _currentDelta += Time.deltaTime / _animationDuration;
            yield return delay;
        }
    }
}
