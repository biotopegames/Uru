using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Item")]
public class Item : ScriptableObject
{
    // Add fields and methods specific to items here

    public Sprite itemSprite;
    public string itemName;

    [SerializeField]
    private ItemType itemType;
    [SerializeField]
    public string itemInfo;
    public int healAmount;
    public int damage;
    public int defense;
    public float attackSpeed;
    public int hpRegeneration;
    public GameObject companionHatchling;

    public enum ItemType
    {
        Rune,
        QuestItem,
        Consumable,
        Equipment,
        Egg,
    }

    public ItemType GetItemType()
    {
        return itemType;
    }
}