using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField]private Stats stats;
    public Slider slider;
    [SerializeField] private Slider staminaSlider;
    public bool isMinion;
    [SerializeField] private GameObject companionHealthBar; // If it's a companion and health is full don't showhealthbar

    void Awake()
    {
        if(isMinion)
        {
        companionHealthBar = GameObject.Find("Canvas/Slider");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!isMinion){
        stats = PlayerController.Instance.GetComponent<Stats>();
        }
        else
        {
        stats = GetComponentInParent<Stats>();
        }
        slider.maxValue = stats.fullHealth; // Set the maxValue of the slider to stats.health
        if(staminaSlider != null)
        staminaSlider.maxValue = stats.maxStamina; // Set the maxValue of the slider to stats.health


    }

    // Update is called once per frame
    void Update()
    {

        slider.value = stats.health;

        if (isMinion)
        {
        if(stats.health == stats.fullHealth)
        {
            companionHealthBar.SetActive(false);
        }
        else
        {
            companionHealthBar.SetActive(true);
        }

        
        slider.maxValue = stats.fullHealth; // Set the maxValue of the slider to stats.health

            if (transform.parent.localScale.x < 0)
            {
                // Set the local scale of the Healthbar GameObject to (1, 1, 1)
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // Set the local scale of the Healthbar GameObject to (-1, 1, 1)
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
        }
        else
        {
        staminaSlider.value = stats.stamina;
        slider.maxValue = stats.fullHealth; // Set the maxValue of the slider to stats.health
        staminaSlider.maxValue = stats.maxStamina; // Set the maxValue of the slider to stats.health
        }
    }
}
