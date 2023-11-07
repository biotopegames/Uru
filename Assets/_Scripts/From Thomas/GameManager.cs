using System.Collections;
using System.Collections.Generic;
using TMPro;
// using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;


/*Manages inventory, keeps several component references, and any other future control of the game itself you may need*/

public class GameManager : MonoBehaviour
{
    public AudioSource audioSource; //A primary audioSource a large portion of game sounds are passed through
    public DialogueBoxController dialogueBoxController;
    private static GameManager instance;
    //[SerializeField] public AudioTrigger gameMusic;



    // Singleton instantiation
    public static GameManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }


    public void LoadSceneCutscene(string sceneName)
    {
        // EditorSceneManager.OpenScene(sceneName);
    }

    // Method to load a scene by name in playmode
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1;

        // Get the index of the current active scene.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        HUD.Instance.CloseAllMenus();
        PlayerController.Instance.stats.health = PlayerController.Instance.stats.fullHealth;
        // Load the current scene by its index, effectively reloading it.
        PlayerController.Instance.transform.position = PlayerController.Instance.respawnPosition;
        SceneManager.LoadScene(currentSceneIndex);
    }


    public static void CloseApplication()
    {
        Application.Quit();
    }


}
