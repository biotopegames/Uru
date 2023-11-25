using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{

    public AudioClip deathSound;
    public AudioClip grassSound;
    public AudioClip dirtSound;

    public AudioClip hurtSound;
    public AudioClip[] hurtSounds;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip attackSound;
    public AudioClip bigAttackSound;
    public AudioClip bigAttackSlamSound;

    public AudioClip[] poundActivationSounds;
    public AudioClip stepSound;
    [System.NonSerialized] public int whichHurtSound;
    [SerializeField] private AudioSource audioSource;
    [System.NonSerialized] public string groundType = "grass";
    private float originalVolume;










    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void AttackEffect()
    {
        //As long as the player as activated the pound in ActivatePound, the following will occur when hitting the ground.

        // animator.ResetTrigger("attack");
        // velocity.y = jumpPower / 1.4f;
        // animator.SetBool("pounded", true);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(attackSound, 0.4f);
        // cameraEffects.Shake(200, 1f);
        // pounding = false;
        // recoveryCounter.counter = 0;
        // animator.SetBool("pounded", true);
    }

    public void BigAttackSound()
    {
        audioSource.PlayOneShot(bigAttackSound);

    }

    public void PlayHurtSound()
    {
        audioSource.PlayOneShot(hurtSound);

    }


    public void PlayStepSound()
    {
        //Play a step sound at a random pitch between two floats, while also increasing the volume based on the Horizontal axis
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        //audioSource.PlayOneShot(stepSound, Mathf.Abs(Input.GetAxis("Horizontal") / 5));
        audioSource.PlayOneShot(stepSound, 1.4f);
    }

    public void PlayLandSound()
    {
        //Play a step sound at a random pitch between two floats, while also increasing the volume based on the Horizontal axis
        audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.volume += 0.5f;
        //audioSource.PlayOneShot(stepSound, Mathf.Abs(Input.GetAxis("Horizontal") / 5));
        audioSource.PlayOneShot(landSound, 2f);
                audioSource.volume -= 0.5f;


    }

    public void PlayJumpSound()
    {

        // audioSource.pitch = (Random.Range(1f, 1f));
        audioSource.PlayOneShot(jumpSound, 1.4f);

    }
}
