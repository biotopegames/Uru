using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*Triggers a dialogue conversation, passing unique commands and information to the dialogue box and inventory system for fetch quests, etc.*/

public class DialogueTrigger : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Animator iconAnimator; //The E icon animator

    [Header("Trigger")]
    [SerializeField] private bool autoHit; //Does the player need to press the interact button, or will it simply fire automatically?
    public bool completed;
    [SerializeField] private bool repeat; //Set to true if the player should be able to talk again and again to the NPC. 
    [SerializeField] private bool sleeping;

    [Header("Dialogue")]
    [SerializeField] private string characterName; //The character's name shown in the dialogue UI
    [SerializeField] private string dialogueStringA; //The dialogue string that occurs before the fetch quest
    [SerializeField] private string dialogueStringB; //The dialogue string that occurs after fetch quest
    [SerializeField] private AudioClip[] audioLinesA; //The audio lines that occurs before the fetch quest
    [SerializeField] private AudioClip[] audioLinesB; //The audio lines that occur after the fetch quest
    [SerializeField] private AudioClip[] audioChoices; //The audio lines that occur when selecting an audio choice

    [Header("Fetch Quest")]
    //[SerializeField] private GameObject deleteGameObject; //If an NPC is holding the object, and gives it to you, this object will destroy
    [SerializeField] private string addWhichItemStringSecond; //The inventory item given if items is fetched
    [SerializeField] private string addWhichItemString; //The inventory item given if items is fetched
    [SerializeField] private string finishTalkingAnimatorBool; //After completing a conversation, an animation can be fired
    [SerializeField] private string finishTalkingActivateObjectString; //After completing a conversation, an object's name can be searched for and activated.
    // [SerializeField] private GameObject[] finishTalkingActivateObject; //After completing a conversation, an object can activate. 
    [SerializeField] private bool activateAfterTalk;
    [SerializeField] private bool deactivateAfterTalk;
    [SerializeField] private GameObject[] activateObjectsAfterCompletedQuest; //After completing a quest, an object will be activated
    [SerializeField] private GameObject[] deactivateObjectAfterCompletedQuest;
    [SerializeField] private Item addItem; //The sprite of the inventory item given, shown in HUD
    [SerializeField] private Item addItemSecond; //The sprite of the inventory item given, shown in HUD

    [SerializeField] private AudioClip getSound; //When the player is given an object, this sound will play
    [SerializeField] private bool instantGet; //Player can be immediately given an item the moment the conversation begins
    [SerializeField] private string[] requiredItem; //The required fetch quest item
    public Animator useItemAnimator; //If the player uses an item, like a key, an animator can be fired (ie to open a door)
    [SerializeField] private string useItemAnimatorBool; //An animator bool can be set to true once an item is used, like ae key.


        // if(dialogueStringA == ""|| dialogueStringB == "")
        // {
        //     Debug.Log("You need to specify the DialogueStringA or DialogueStringB, and remember to add them in the dialogue script");
        // }


    void OnTriggerStay2D(Collider2D col)
    {
        if (instantGet)
        {
            InstantGet();
        }

        if (col.gameObject == PlayerController.Instance.gameObject && !sleeping && !completed)
        {
            iconAnimator.SetBool("active", true);
            if (autoHit || Input.GetKey(KeyCode.W))
            {
                iconAnimator.SetBool("active", false);
                // If you don't have the required items or you dont need any required items
                if (requiredItem.Length == 0 || !InventoryContainsList())
                {
                    HUD.Instance.dialogueBoxController.Appear(dialogueStringA, characterName, this, false, audioLinesA, audioChoices, finishTalkingAnimatorBool, activateObjectsAfterCompletedQuest, finishTalkingActivateObjectString, deactivateObjectAfterCompletedQuest, repeat, activateAfterTalk, deactivateAfterTalk);
                    Debug.Log("Dont contain item1");
                }
                else if (InventoryContainsList())
                {
                    Debug.Log("Dont contain item2");

                    if (dialogueStringB != "")
                    {
                        HUD.Instance.dialogueBoxController.Appear(dialogueStringB, characterName, this, true, audioLinesB, audioChoices, "", null, "", deactivateObjectAfterCompletedQuest, repeat, activateAfterTalk, deactivateAfterTalk);
                    }
                    else
                    {
                        UseItem();
                    }
                }
                sleeping = true;
            }
        }
        else
        {
            iconAnimator.SetBool("active", false);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == PlayerController.Instance.gameObject)
        {
            iconAnimator.SetBool("active", false);
            sleeping = completed;
        }
    }

    public void UseItem()
    {

        if (!completed)
        {
            if (useItemAnimatorBool != "")
            {
                useItemAnimator.SetBool(useItemAnimatorBool, true);
            }

            // if (deleteGameObject)
            // {
            //     Destroy(deleteGameObject);
            // }

            Collect();

            if (InventoryContainsList())
            {
                InventoryRemoveList();
            }
            Debug.Log("Came to useItem");

            if (activateObjectsAfterCompletedQuest.Length > 0 && activateAfterTalk == false) 
            {
                foreach (GameObject activateObject in activateObjectsAfterCompletedQuest)
                {
                    activateObject.SetActive(true);
                }
            }
            
            if (deactivateObjectAfterCompletedQuest.Length > 0 && deactivateAfterTalk == false) 
            {
                foreach (GameObject activateObject in deactivateObjectAfterCompletedQuest)
                {
                    activateObject.SetActive(true);
                }
            }



            repeat = false;
        }

    }

    //Method gets called in the end when quest is completed
    public void Collect()
    {
        if (!completed)
        {
            if (addWhichItemString != "")
            {
                Inventory.Instance.AddInventoryItem(addWhichItemString, addItem);
            }

            if (addWhichItemStringSecond != "")
            {
                Inventory.Instance.AddInventoryItem(addWhichItemStringSecond, addItemSecond);
            }

            if (getSound != null)
            {
                //Inventory.Instance.audioSource.PlayOneShot(getSound);
            }

            completed = true;
            //AfterCompletedQuestActivateObject.SetActive(true);

        }
    }

    public void InstantGet()
    {
        Inventory.Instance.AddInventoryItem(addWhichItemString, null);
        Inventory.Instance.AddInventoryItem(addWhichItemStringSecond, null);
        instantGet = false;
    }


    public bool InventoryContainsList()
    {
        foreach (string requiredItem in requiredItem)
        {
            if (!Inventory.Instance.inventory.ContainsKey(requiredItem))
            {
                return false;
            }
        }

        return true;
    }

    public void InventoryRemoveList()
    {
        foreach (string requiredItems in requiredItem)
        {
            Inventory.Instance.inventory.Remove(requiredItems);
        }
    }

}