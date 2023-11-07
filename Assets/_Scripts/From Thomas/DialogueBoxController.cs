using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

/*Controls the dialogue box and it's communication with Dialogue.cs, which contains the character dialogue*/

public class DialogueBoxController : MonoBehaviour
{

    [Header("References")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioSource dialogueAudioSource;
    [SerializeField] Dialogue dialogue;
    private DialogueTrigger currentDialogueTrigger;

    [Header("Sounds")]
    private AudioClip[] audioLines;
    private AudioClip[] audioChoices;
    [SerializeField] private AudioClip selectionSound;
    [SerializeField] private AudioClip[] typeSounds;
    [SerializeField] private float typeSoundsVolumeMin;
    [SerializeField] private float typeSoundsVolumeMax;

    [Header("Text Mesh Pro")]
    [SerializeField] TextMeshProUGUI choice1Mesh;
    [SerializeField] TextMeshProUGUI choice2Mesh;
    [SerializeField] TextMeshProUGUI nameMesh;
    [SerializeField] TextMeshProUGUI textMesh;
    [Header("Time between each character")]

    [SerializeField] private float typeSpeed = 1f;


    [Header("Other")]
    private bool ableToAdvance;
    private bool activated;
    private int choiceLocation;
    private int cPos = 0;
    private bool deactivateAfterTalk;
    private bool activateAfterTalk;
    private string[] characterDialogue;
    private string[] choiceDialogue;
    private DialogueTrigger dialogueTrigger;
    [System.NonSerialized] public bool extendConvo;
    private string finishTalkingAnimatorBool;
    private GameObject[] finishTalkDeactivateGameObjects;
    private GameObject[] finishTalkingActivateGameObjects;
    private string finishTalkingActivateGameObjectString;
    private string fileName;
    private int index = -1;
    private bool repeat;
    private bool horizontalKeyIsDown = true;
    private bool submitKeyIsDown = true;
    private bool typing = true;

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            //Submit
            //Check for key press
            if (((Input.GetAxis("Submit") > 0) || (Input.GetAxis("Jump") > 0)) && !submitKeyIsDown)
            {
                submitKeyIsDown = true;
                if (!typing)
                {
                    if (index < choiceLocation || (extendConvo && index < characterDialogue.Length - 1))
                    {
                        if (ableToAdvance)
                        {
                            StartCoroutine(Advance());
                        }
                    }
                    else
                    {
                        StartCoroutine(Close());
                    }
                    if (index == 0)
                    {
                        ableToAdvance = true;
                    }
                }
            }

            //Check for first release to ensure we can't spam
            if (submitKeyIsDown && Input.GetAxis("Submit") < .001 && Input.GetAxis("Jump") < .001)
            {
                if (!typing)
                {
                    submitKeyIsDown = false;
                    if (index == 0)
                    {
                        ableToAdvance = true;
                    }
                }
            }

            //Choices

            //Check for key press
            if ((Input.GetAxis("Horizontal") != 0) && !horizontalKeyIsDown && animator.GetBool("hasChoices") == true)
            {
                if (animator.GetInteger("choiceSelection") == 1)
                {
                    animator.SetInteger("choiceSelection", 2);
                    extendConvo = true;
                }
                else
                {
                    extendConvo = false;
                    animator.SetInteger("choiceSelection", 1);
                }

                audioSource.PlayOneShot(selectionSound, 0.2f);
                horizontalKeyIsDown = true;
            }

            //Checks if its choice1 you pressed
            if (animator.GetInteger("choiceSelection") == 1 && Input.GetButtonDown("Submit") && animator.GetBool("active") == false)
            {
                Debug.Log("Submit for Choice1");
                //Checks if the current dialogue is AlterB
                if (fileName == "AlterB")
                {
                    Debug.Log("Submit for Choice1 FOR THE AlterB");
                    //Assign the comanion gameobject or "the hatchling" from the egg to the Player so he can summon it
                    
                    //Item item = Inventory.Instance.inventory["Egg"];
                    //PlayerController.Instance.companionGameobject = item.companionHatchling;
                    //Inventory.Instance.RemoveInventoryItem("Egg");
                    //Inventory.Instance.UpdateSlots();
                    //Set player hasCompanion to true because it determines wether or not he can release the companion
                    PlayerController.Instance.hasCompanion = true;
                }
            }

            if (animator.GetInteger("choiceSelection") == 2 && Input.GetButtonDown("Submit") && animator.GetBool("active") == false && fileName == "AlterB")
            {
                // Close the dialogue without completing it.
                StartCoroutine(Close());
            }

            //Check for first release to ensure we can't spam
            if (horizontalKeyIsDown && Input.GetAxis("Horizontal") == 0)
            {
                horizontalKeyIsDown = false;
            }
        }
    }

    /*   This method is called from DialogueTrigger and gets all the input arguments.
         Then calls Advance() method. But also calls GetChoiceLocation() if theres a choice in the dialogue*/
    public void Appear(string fName, string characterName, DialogueTrigger dTrigger, bool useItemAfterClose, AudioClip[] audioL, AudioClip[] audioC, string finishTalkingAnimBool, GameObject[] finishTalkingActivateGO, string finishTalkingActivateGOString, GameObject[] finishTalkDeactivateGO, bool r, bool a, bool d)
    {

        finishTalkDeactivateGameObjects = finishTalkDeactivateGO;
        finishTalkingActivateGameObjects = finishTalkingActivateGO;
        activateAfterTalk = a;
        deactivateAfterTalk = d;
        repeat = r;
        finishTalkingAnimatorBool = finishTalkingAnimBool;
        finishTalkingActivateGameObjectString = finishTalkingActivateGOString;
        dialogueTrigger = dTrigger;
        choice1Mesh.text = "";
        choice2Mesh.text = "";
        fileName = fName;
        audioLines = audioL;
        audioChoices = audioC;

        if (useItemAfterClose)
        {
            currentDialogueTrigger = dialogueTrigger;
        }

        nameMesh.text = characterName;
        characterDialogue = dialogue.dialogue[fileName];

        if (dialogue.dialogue.ContainsKey(fileName + "Choice1"))
        {
            choiceDialogue = dialogue.dialogue[fileName + "Choice1"];
            choiceLocation = GetChoiceLocation();
        }
        else
        {
            choiceLocation = characterDialogue.Length - 1;
        }

        animator.SetInteger("choiceSelection", 1);
        animator.SetBool("active", true);
        activated = true;
        PlayerController.Instance.anim.SetBool("isRunning", false);
        PlayerController.Instance.frozen = true;
        StartCoroutine(Advance());
    }

    IEnumerator Close()
    {
        if (index == choiceLocation && dialogue.dialogue.ContainsKey(fileName + "Choice1") && audioChoices.Length != 0)
        {
            audioSource.Stop();
            yield return new WaitForSeconds(.1f);
            if (animator.GetInteger("choiceSelection") == 1)
            {
                dialogueAudioSource.PlayOneShot(audioChoices[0]);
            }
            else
            {
                dialogueAudioSource.PlayOneShot(audioChoices[1]);
            }
        }

        //The dialogueTrigger will pass itself into this function only if you have the right items to close the dialogue and complete the quest
        if (currentDialogueTrigger != null)
        {
            currentDialogueTrigger.UseItem();
        }

        // if (currentDialogueTrigger != null && fileName != "AlterB" && animator.GetInteger("choiceSelection") == 2)
        // {
        //     currentDialogueTrigger.UseItem();
        // }

        activated = false;
        animator.SetBool("active", false);
        StopCoroutine("TypeText");
        index = -1;
        submitKeyIsDown = false;
        ableToAdvance = false;
        extendConvo = false;
        choiceLocation = 0;
        ShowChoices(false);

        if (finishTalkingAnimatorBool != "")
        {
            dialogueTrigger.GetComponent<DialogueTrigger>().useItemAnimator.SetBool(finishTalkingAnimatorBool, true);
        }

        try
        {
            if (finishTalkDeactivateGameObjects.Any() && deactivateAfterTalk)
            {
                foreach (GameObject activateObject in finishTalkDeactivateGameObjects)
                {
                    activateObject.SetActive(false);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        // So only deactivate if the flag deactiveAfterTalk is set from the trigger


        try
        {
            // So only activate if the flag deactiveAfterTalk is set from the trigger
            if (finishTalkingActivateGameObjects.Length > 0 && activateAfterTalk)
            {
                foreach (GameObject go in finishTalkingActivateGameObjects)
                {
                    go.SetActive(true);
                }
            }
        }
        catch(System.Exception e)
        {
                Debug.Log(e.Message);
        }

        if (finishTalkingActivateGameObjectString != "" && finishTalkingActivateGameObjectString != null)
        {
            //GameObject.Find(finishTalkingActivateGameObjectString).SetActive(true);
            //dialogueTrigger.completed = true;
        }

        if (!repeat)
        {
            dialogueTrigger.completed = true;
        }

        dialogueTrigger = null;
        finishTalkingAnimatorBool = "";
        finishTalkingActivateGameObjects = null;
        finishTalkingActivateGameObjectString = "";
        yield return new WaitForSeconds(1f);
        PlayerController.Instance.frozen = false;
        animator.SetInteger("choiceSelection", 1);
    }

    IEnumerator Advance()
    {
        index++;
        typing = true;

        if (ableToAdvance)
        {
            animator.SetTrigger("press");
        }

        if (index != choiceLocation)
        {
            ShowChoices(false);
        }

        if (index == choiceLocation + 1 && dialogue.dialogue.ContainsKey(fileName + "Choice1") && audioChoices.Length != 0)
        {
            audioSource.Stop();
            yield return new WaitForSeconds(.1f);
            if (animator.GetInteger("choiceSelection") == 1)
            {
                dialogueAudioSource.PlayOneShot(audioChoices[0]);
                yield return new WaitForSeconds(audioChoices[0].length);
            }
            else
            {
                dialogueAudioSource.PlayOneShot(audioChoices[1]);
                yield return new WaitForSeconds(audioChoices[1].length);
            }
        }

        textMesh.text = "";
        StartCoroutine("TypeText");

        //Wait before typing
        yield return new WaitForSeconds(.4f);

        //Show choices
        if (index == choiceLocation && dialogue.dialogue.ContainsKey(fileName + "Choice1"))
        {
            ShowChoices(true);
        }

        //Play character audio
        if (audioLines.Length != 0)
        {
            dialogueAudioSource.Stop();
            if (index < audioLines.Length)
            {
                // this determines how often the type sound is played
                if (audioLines[index] != null)
                {
                    dialogueAudioSource.PlayOneShot(audioLines[index]);
                }
            }
        }
    }

    IEnumerator TypeText()
    {
        WaitForSeconds wait = new WaitForSeconds(typeSpeed);
        foreach (char c in characterDialogue[index])
        {
            cPos++;
            if (cPos != 0 && cPos <= characterDialogue[index].Length)
            {
                typing = false;
                cPos = 0;
            }

            textMesh.text += c;
            audioSource.PlayOneShot(typeSounds[Random.Range(0, typeSounds.Length)], Random.Range(typeSoundsVolumeMin, typeSoundsVolumeMax));
            yield return wait;
        }
    }

    public int GetChoiceLocation()
    {
        Debug.Log("Got into get choice location");
        for (int i = 0; i < choiceDialogue.Length; i++)
        {
            if (choiceDialogue[i] != "")
            {
                return i;
            }
        }
        return 0;
    }

    void ShowChoices(bool show)
    {
        animator.SetBool("hasChoices", show);
        if (show)
        {
            choice1Mesh.text = dialogue.dialogue[fileName + "Choice1"][choiceLocation];
            choice2Mesh.text = dialogue.dialogue[fileName + "Choice2"][choiceLocation];
        }
    }
}
