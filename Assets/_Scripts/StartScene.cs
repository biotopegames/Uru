using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public string sceneToLoad = "YourSceneName";

    void Start()
    {
        // Load the specified scene
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
