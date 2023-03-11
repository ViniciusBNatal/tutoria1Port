using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventLook : MonoBehaviour
{
    public AudioSource sound;
    public GameObject toEnable;
    public Rigidbody rdb;
    public Vector3 force;
    [SerializeField] private CollectableItemData _itemRequired;

    //funcao que é chamada depois de um tempo olhando
    public void ButtonAction(InventoryUI itemNeeded)
    {
        if (_itemRequired != null && itemNeeded.CurrentItemID != _itemRequired.ItemID)
        {
            itemNeeded.TriggerAnimation(_itemRequired.ItemID);
            return;
        }
        //toca o som escolhido
        if (sound)
        {
            sound.Play();
        }
        //habilita gameobjec selecionado
        if (toEnable)
        {
            toEnable.SetActive(true);
        }
        //adiciona uma força no objeto selecionado
        if (rdb)
        {
            rdb.AddForce(force, ForceMode.Impulse);
        }


    }

    //se acontece uma colisao toca o som
    private void OnCollisionEnter(Collision collision)
    {
        if (sound)
        {
            sound.Play();
        }
    }

}
