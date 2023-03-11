using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private CollectableItemData _data;
    public CollectableItemData Data => _data;

    public void Collect(CollectableItem collectableItem)
    {
        Destroy(gameObject);
    }
}
