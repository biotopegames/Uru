using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private Inventory inventory;
    public Item item;

    // Start is called before the first frame update
    void Start()
    {
        inventory = PlayerController.Instance.GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.Log("Inventory script is missing breddah");
        }
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {


            Rune rune = item as Rune;
            if (rune != null)
            {
                for (int i = 0; i < inventory.runesSlots.Length; i++)
                {
                    if (!inventory.isRuneFull[i])
                    {
                        inventory.isRuneFull[i] = true;
                        inventory.runesSlots[i].gameObject.GetComponent<Slot>().SetSlotItem(item, rune.itemSprite);
                        Pickup();
                        break;
                    }

                }
            }
            else
            {
                for (int i = 0; i < inventory.itemsSlots.Length; i++)
                {
                    if (!inventory.isItemFull[i])
                    {
                        inventory.isItemFull[i] = true;
                        inventory.itemsSlots[i].GetComponent<Slot>().SetSlotItem(item, item.itemSprite);
                        Pickup();
                        break;
                    }
                }
            }
        }
    }

    public void Pickup()
    {
        HUD.Instance.ShowInfoText("+1 " + item.itemName);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        // Here you can put code to pick up the item animations etc.
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        Destroy(gameObject, 0.2f);
    }
}