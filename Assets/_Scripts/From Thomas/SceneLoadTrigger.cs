using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Loads a new scene, while also clearing level-specific inventory!*/

public class SceneLoadTrigger : MonoBehaviour
{


    [SerializeField] private bool shouldPressE;
    [SerializeField] private bool hasPressedE;
    [SerializeField] private Animator iconAnimator; //The E icon animator
    [SerializeField] private PlayerPosition playerPosition;
    public float playerPosXInNextScene;
    public float playerPosYInNextScene;


    [SerializeField] string loadSceneName;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == PlayerController.Instance.gameObject)
        {
            if (!shouldPressE) {
                // GameManager.Instance.hud.loadSceneName = loadSceneName;
                // GameManager.Instance.inventory.Clear();
                // GameManager.Instance.hud.animator.SetTrigger("coverScreen");
                // enabled = false;
                playerPosition.x = playerPosXInNextScene;
                playerPosition.y = playerPosYInNextScene;
                HUD.Instance.anim.SetTrigger("coverScreen");
                StartCoroutine(WaitToLoad());

            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject == PlayerController.Instance.gameObject)
        {
            if (iconAnimator != null)
            {
                if (shouldPressE)
                {
                    iconAnimator.SetBool("active", true);
                }
                else
                {
                    iconAnimator.SetBool("active", false);
                }
            }
            else
            {

                //GameManager.Instance.LoadScene(loadSceneName);
            }
            // if (shouldPressE && hasPressedE)
            // {
            //     GameManager.Instance.hud.loadSceneName = loadSceneName;
            //     GameManager.Instance.inventory.Clear();
            //     GameManager.Instance.hud.animator.SetTrigger("coverScreen");
            //     enabled = false;
            // }
        }
    }

    IEnumerator WaitToLoad()
    {
        yield return new WaitForSeconds(1);
        PlayerController.Instance.transform.position = new Vector2(playerPosXInNextScene, playerPosYInNextScene);
        GameManager.Instance.LoadScene(loadSceneName);

    }

    private void Update()
    {



        // if(NewPlayer.Instance.isPressingE)
        // {
        //     hasPressedE = true;
        // }
        // else
        // {
        //     hasPressedE = false;
        // }
    }
}
