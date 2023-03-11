using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableItemData", menuName = "Inventory/ItemData")]
public class CollectableItemData : ScriptableObject
{
    [SerializeField] private Sprite _itemIcon;
    public string ItemID => this.name;
    public Sprite ItemIcon => _itemIcon;
}
