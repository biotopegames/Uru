using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class CompanionUI : MonoBehaviour
{

[SerializeField] private Animator anim;
public TextMeshProUGUI statusText;

void Start()
{
    anim = GetComponent<Animator>();
}

void Update()
{
    if(Input.GetKey(KeyCode.U))
    anim.SetTrigger("gainExp");
}

    // Start is called before the first frame update

public void GainExpCompanionUI(int gainedExp)
{
statusText.text = gainedExp.ToString() + "XP";
anim.SetTrigger("gainExp");
}
}
