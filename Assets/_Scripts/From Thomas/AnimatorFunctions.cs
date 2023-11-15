using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*This script can be used on pretty much any gameObject. It provides several functions that can be called with 
animation events in the animation window.*/

public class AnimatorFunctions : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource hatchMusic;

    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Animator setBoolInAnimator;
    [SerializeField] private float targetYOffset;

    // If we don't specify what audio source to play sounds through, just use the one on player.
    void Start()
    {
        // if (!audioSource) audioSource = NewPlayer.Instance.audioSource;
    }

    //Hide and unhide the player
    public void HidePlayer(bool hide)
    {
        // NewPlayer.Instance.Hide(hide);
    }
  
    //Sometimes we want an animated object to force the player to jump, like a jump pad.
    public void JumpPlayer(float power = 1f)
    {
        // NewPlayer.Instance.Jump(power);
    }

    //Freeze and unfreeze the player movement
    void FreezePlayer(bool freeze)
    {
        // PlayerController.Instance.Freeze(freeze);
    }

    //Play a sound through the specified audioSource
    void PlaySound(AudioClip whichSound)
    {
        audioSource.PlayOneShot(whichSound);
    }

        void StopSound()
    {
        hatchMusic.Stop();
    }

    public void EmitParticles(int amount)
    {
        // if (!particleSystem) return;


        Instantiate(particleSystem, transform.position , Quaternion.identity);

        // instantiateParticles(amount);
        particleSystem.Emit(amount);
    }

    public void ScreenShake(float power)
    {
        // NewPlayer.Instance.cameraEffects.Shake(power, 1f);
    }

    public void SetTimeScale(float time)
    {
        Time.timeScale = time;
    }

    // public void LoadSceneWithPosition(string sceneName, PlayerPosition position, float playerPosXInNextScene, float playerPosYInNextScene)
    // {
    //     position.x = playerPosXInNextScene;
    //     position.y = playerPosYInNextScene;
    //     PlayerController.Instance.transform.position = new Vector2(playerPosXInNextScene, playerPosYInNextScene);
    //     GameManager.Instance.LoadScene(sceneName);
    // }

    //     public void LoadScene(string sceneName)
    // {
    //     GameManager.Instance.LoadScene(sceneName);
    // }

    public void ShootArrow()
    {
    if (GetComponentInParent<Enemy>() != null)
    {
        Enemy enemy = GetComponentInParent<Enemy>();
        // Calculate the direction to the target.
        Vector3 targetPosition = new Vector3(enemy.GetTarget().transform.position.x, enemy.GetTarget().transform.position.y + targetYOffset);
        Vector3 direction = targetPosition - enemy.GetArrowSpawnPos().transform.position;
        direction.Normalize();

        // Instantiate the arrow at the spawn position.
        GameObject arrow = Instantiate(enemy.GetArrowPrefab(), enemy.GetArrowSpawnPos().position, Quaternion.identity);

        //Set the arrows damage to the enemys'
        arrow.GetComponent<Arrow>().damage = enemy.stats.damage;

        // Calculate the rotation angle to point toward the target.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Get the Rigidbody2D component of the arrow.
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        // Set the arrow's velocity to move toward the target.
        rb.velocity = direction * arrow.GetComponent<Arrow>().arrowSpeed;
    }
    }

    IEnumerable arroWWait()
    {
        yield return new WaitForSeconds(0.2f);
    }

    public void SetAnimBoolToFalse(string boolName)
    {
        setBoolInAnimator.SetBool(boolName, false);
    }

    public void SetAnimBoolToTrue(string boolName)
    {
        setBoolInAnimator.SetBool(boolName, true);
    }

    // public void FadeOutMusic()
    // {
    //    GameManager.Instance.gameMusic.GetComponent<AudioTrigger>().maxVolume = 0f;
    // }

    public void LoadScene(string whichLevel)
    {
        SceneManager.LoadScene(whichLevel);
    }

    //Slow down or speed up the game's time scale!
    public void SetTimeScaleTo(float timeScale)
    {
        Time.timeScale = timeScale;
    }
}
    