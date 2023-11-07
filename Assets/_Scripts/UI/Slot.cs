using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject optionsButtons; // The options buttons object
    public GameObject infoObject; // The info object to display
    private bool isOptionsOpen = false; // Flag to track if the options are open
    public Image itemSlotImage;
    public Image emptySlotImage;
    public GameObject useOption;
    public GameObject equipOption;
    public Item slotItem; // The item in the slot
    private Image itemRimImage;
    private Color originalRimColor;
    private Color equippedRimColor;
    private bool itemIsEquipped;
    private AudioSource audio;


    void Start()
    {
        audio = GetComponent<AudioSource>();
        // Hide the options buttons and info object at the start
        optionsButtons.SetActive(false);
        infoObject.SetActive(false);
        itemRimImage = GetComponent<Image>();
        originalRimColor = itemRimImage.color;
        equippedRimColor = new Color32(0x12, 0xFF, 0x00, 0xFF);
    }

    public void ButtonClick()
    {
        if(slotItem != null)
        {
        isOptionsOpen = true;
        optionsButtons.SetActive(isOptionsOpen);
        }
    }

    public void CancelOptions()
    {
        if (isOptionsOpen)
        {
            // Close the options buttons
            optionsButtons.SetActive(false);
            isOptionsOpen = false;

            // Disable the info object
            infoObject.SetActive(false);
        }
    }

    // Method is used to set the color of the rim
    public void SetSlotItem(Item item, Sprite itemSprite){
        slotItem = item;
        itemSlotImage.sprite = itemSprite;
        Inventory.Instance.AddInventoryItem(slotItem.itemName, slotItem);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Check if the options buttons are open
        if(slotItem != null)
        {
        infoObject.SetActive(true);
        infoObject.GetComponentInChildren<TextMeshProUGUI>().text = slotItem.itemInfo;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Check if the options buttons are open
        if (isOptionsOpen)
        {
            // Disable the info object
            CancelOptions();

        }
        infoObject.SetActive(false);
    }

    public void DropItem()
    {
        if (slotItem != null)
        {
            if(Inventory.Instance.inventory.ContainsKey(slotItem.itemName))
            {
            Inventory.Instance.RemoveInventoryItem(slotItem.itemName);
            }
            slotItem = null;
            itemSlotImage.sprite = emptySlotImage.sprite;

            CancelOptions();
            itemIsEquipped = false;
        }
    }

    public void UseItem()
    {
        if (slotItem != null && slotItem.GetItemType() == Item.ItemType.Consumable)
        {
            PlayerController.Instance.stats.Heal(slotItem.healAmount);

            if (slotItem.GetItemSound() != null)
            {
                audio.PlayOneShot(slotItem.GetItemSound());
            }

            slotItem = null;
            itemSlotImage.sprite = emptySlotImage.sprite;
            CancelOptions();
            // Display some heal animation
        }
    }

    public void EquipItem()
    {

        CancelOptions();
        if (slotItem != null && slotItem.GetItemType() == Item.ItemType.Rune)
        {
            if (slotItem.GetItemSound() != null)
            {
                audio.PlayOneShot(slotItem.GetItemSound(), 0.2f);
            }
            Rune rune = (Rune)slotItem;

            if (!itemIsEquipped)
            {
                PlayerController.Instance.stats.fullHealth += (rune.hp);
                PlayerController.Instance.stats.stamina += (rune.staminaAmount);

                itemIsEquipped = true;
                SlotEquipped();
            }
            else
            {
                PlayerController.Instance.stats.fullHealth -= (rune.hp);
                PlayerController.Instance.stats.stamina -= (rune.staminaAmount);

                itemIsEquipped = false;
                SlotEquipped();
            }

        }
    }

    public void SlotEquipped()
    {
        if (itemIsEquipped){
        itemRimImage.color = equippedRimColor;
        equipOption.GetComponentInChildren<TextMeshProUGUI>().text = "unequip";
        }
        else{
        itemRimImage.color = originalRimColor;
        equipOption.GetComponentInChildren<TextMeshProUGUI>().text = "equip";
        }
    }

}