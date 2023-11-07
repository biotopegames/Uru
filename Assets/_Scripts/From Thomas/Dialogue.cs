using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This script stores every dialogue conversation in a public Dictionary.*/

public class Dialogue : MonoBehaviour
{

    public Dictionary<string, string[]> dialogue = new Dictionary<string, string[]>();

    void Start()
    {
        //Door
        dialogue.Add("LockedDoorA", new string[] {
            "A large door...",
            "Looks like it has a key hole!"
        });


        dialogue.Add("LockedDoorB", new string[] {
            "Key used!"
        });

        //House door
        dialogue.Add("HouseDoorA", new string[] {
            "Press E to enter the house.."
        });

        //NPC
        dialogue.Add("WeirdoA", new string[] {
            "Bombaclaat press 'I' for the info menu."
        });


        //NPC
        dialogue.Add("SignA", new string[] {
            "They DESTROYED everything... there's no guardian creatures to defend us anymore.",
            "If you can get me the 3 bug totems i might be able to summon one more."
        });

        //NPC
        dialogue.Add("SignB", new string[] {
            "Congratulations you found all totems!",
            "Here take this egg! you can hatch at the alter!"
        });

        //NPC
        dialogue.Add("AlterA", new string[] {
            "You need an egg to use The Alter of Incubation",
        });

        //NPC
        dialogue.Add("AlterB", new string[] {
            "You found an egg! If you hatch it, you can press 'R' to summon it!",
            "Do you want to hatch your egg!",

        });

        dialogue.Add("AlterBChoice1", new string[] {
                "",
                "Yes"
            });

        dialogue.Add("AlterBChoice2", new string[] {
                "",
                "No"
            });

        //Boat
        dialogue.Add("Boat", new string[] {
            "You've cleared the island, do you want to go to next one?"
        });

        dialogue.Add("BoatChoice1", new string[] {
                "Go to next island"
            });

        dialogue.Add("BoatChoice2", new string[] {
                "Stay here"
            });
    }

    /*
        //Door
        dialogue.Add("LockedDoorA", new string[] {
                "A large door...",
                "Looks like it has a key hole!"
            });


    dialogue.Add("LockedDoorB", new string[] {
                "Key used!"
            });

    //House door
    dialogue.Add("HouseDoorA", new string[] {
                "Press E to enter the house.."
            });



    //NPC
    dialogue.Add("CharacterA", new string[] {
                "Greetings... [BURP..]",
                "Welcome to utgaard the land of Jaetterne.",
                "I've been stuck here for years.. and theese damn spiders keeps pestering me...",
                "Can you help me get rid of them? bring me the 20 coins they've stolen, and i'll give you the key to gate.",
                "Well suit yourself, you will be stuck here for ages..."
            });

    //The line you write here corresponds to where in the conversation the choice should come
    dialogue.Add("CharacterAChoice1", new string[] {
                "",
                "",
                "",
                "Sure I'll get rid of them!",
            });

    //If this choice is selected it will continue the conversation
    dialogue.Add("CharacterAChoice2", new string[] {
                "",
                "",
                "",
                "Nah.. I don't trust drunkards"
            });

    dialogue.Add("CharacterB", new string[] {
                "Hey! you did it! Many thanks friend!",
                "Here's the key! also feel free to grab some food inside!",
                " I've also taught you a new ability DOWNWARD SMASH Simply attack while pressing down in mid-air!",
                "Safe travels! "
            });
        }*/



}
