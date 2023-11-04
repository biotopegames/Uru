using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private static Inventory instance;

    public bool [] isItemFull;
    public int coin;
    public bool [] isRuneFull;
    public Dictionary<string, Item> inventory = new Dictionary<string, Item>();


    public GameObject [] itemsSlots; 
    public GameObject [] runesSlots; 

        // Singleton instantiation
    public static Inventory Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<Inventory>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    public void RemoveInventoryItem(string name)
    {
        inventory.Remove(name);
        //HUD.Instance.SetInventoryImage(hud.blankUI);
        UpdateSlots();
    }

    public void ClearInventory()
    {
        inventory.Clear();
        //HUD.Instance.SetInventoryImage(hud.blankUI);
    }

        public void AddInventoryItem(string itemName, Item item)
    {
        if(!inventory.ContainsKey(itemName))
        {
        inventory.Add(itemName, item);
        Debug.Log(itemName + " has been added to the inventory");
        Debug.Log(inventory.ContainsKey(itemName));
        
        //find a slot to add the item and SetSlotItem with the item and sprite

        }
        UpdateSlots();

    }

public void UpdateSlots()
{
        for(int i = 0; i < itemsSlots.Length; i++)
    {
        Slot slot = itemsSlots[i].GetComponent<Slot>();
        
        // Check if the inventory doesn't contain the key but there is an item in the slot
        if ((slot.slotItem != null) && !inventory.ContainsKey(slot.slotItem.itemName))
        {
            // Call DropItem on the slot
            isItemFull[i] = false;
            slot.DropItem();
        }

        // Check if there isn't an item 
        // if(slot.slotItem == null && )
        // {
        //     isItemFull[i] = true;
        // }
        i++;
    }



    // Iterate through itemsSlots
    // foreach (GameObject slot in itemsSlots)
    // {
    //     Slot itemSlot = slot.GetComponent<Slot>();
        
    //     // Check if the inventory doesn't contain the key
    //     if (!inventory.ContainsKey(itemSlot.slotItem.itemName))
    //     {
    //         // Call DropItem on the slot
    //         itemSlot.DropItem();
    //     }
    // }

    // // Iterate through runesSlots
    // foreach (GameObject slot in runesSlots)
    // {
    //     Slot runeSlot = slot.GetComponent<Slot>();
        
    //     // Check if the inventory doesn't contain the key
    //     if (!inventory.ContainsKey(runeSlot.slotItem.itemName))
    //     {
    //         // Call DropItem on the slot
    //         runeSlot.DropItem();
    //     }
    // }
}

    // Use this for initialization
    // public void GetInventoryItem(string name, Item item)
    // {
    //     inventory.Add(name, item);

    //     if (item != null)
    //     {
    //         //HUD.Instance.SetInventoryImage(inventory[name]);
    //     }
    //     UpdateSlots();
    // }

//     public void UpdateSlots()
// {
//     // // Clear the slots
//     // for (int i = 0; i < itemsSlots.Length; i++)
//     // {
//     //     itemsSlots[i].GetComponent<Slot>().SetSlotItem(null, null);
//     // }

//     // // Add items to the slots
//     // int index = 0;
//     // foreach (var item in inventory.Values)
//     // {
//     //     itemsSlots[index].GetComponent<Slot>().SetSlotItem(item, item.itemSprite);
//     //     index++;
//     // }
// }

        // Method to check if the provided amounts are less than or equal to the available resources
    // public bool CheckResourceAvailability(int requiredCoins, int requiredTrees, int requiredOre)
    // {
    //     // Check if all resource requirements are met
    //     if (requiredCoins <= coin && requiredTrees <= tree && requiredOre <= ore)
    //     {
    //         // All requirements are met, return true
    //         return true;
    //     }
    //     else
    //     {
    //         // At least one requirement is not met, return false
    //         return false;
    //     }
    // }


}
