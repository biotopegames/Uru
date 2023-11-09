using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    public TextMeshProUGUI HpTextMeshProGUI;
    public TextMeshProUGUI def;
    public TextMeshProUGUI atkspeed;
    public TextMeshProUGUI companionLore;
    public TextMeshProUGUI lvl;
    public TextMeshProUGUI titleObject;

    public TextMeshProUGUI name;
    public TextMeshProUGUI damage;
    public TextMeshProUGUI xpTillNextLevel;
    public TextMeshProUGUI currentXp;
    public TextMeshProUGUI infoText;

    public TextMeshProUGUI xpPoints;
    public Stats companionStats;
    public GameObject Inventory;
    public bool isInventoryOpen = false;
    public GameObject creatureTab;
    public GameObject runesTab;
    public GameObject itemsTab;
    public Button creatureTabButton;
    public Button runesTabButton;
    public Button itemsTabButton;
    public TextMeshProUGUI currentHpText;
    public TextMeshProUGUI maxHpText;
    public TextMeshProUGUI currentStaminaText;
    public TextMeshProUGUI maxStaminaText;
    private PlayerController player;
    public DialogueBoxController dialogueBoxController;
    public Animator anim;
    public GameObject infoObject;
    public GameObject deadMenu;
    private Healthbar healthbar;


    // Singleton instantiation

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.Instance;
        if(PlayerController.Instance.companionGameobject != null){
        companionStats = PlayerController.Instance.companionGameobject.GetComponent<Stats>();
        }
        anim.SetTrigger("showTitle");
        //anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.Instance.stats.health < 0)
        PlayerController.Instance.stats.health = 0;

        if(PlayerController.Instance.companionGameobject != null && PlayerController.Instance.companionIsOut == true)
        {
        HpTextMeshProGUI.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().health.ToString();
        companionLore.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().lore.ToString();
        def.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().defense.ToString();
        atkspeed.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().attackSpeed.ToString();
        lvl.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().lvl.ToString();
        name.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().name;
        damage.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().damage.ToString();
        xpTillNextLevel.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().xpNeededToLevelUp.ToString();
        currentXp.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().currentXp.ToString();

        xpPoints.text = PlayerController.Instance.companionGameobject.GetComponent<Stats>().xpPoints.ToString();
        }

        currentHpText.text = player.stats.health.ToString();
        maxHpText.text = player.stats.fullHealth.ToString();
        currentStaminaText.text = player.stats.stamina.ToString();
        maxStaminaText.text = player.stats.maxStamina.ToString();        
    }

    public void SpendXpPointHp()
    {
        if (companionStats.xpPoints > 0)
        {
            companionStats.xpPoints -= 1;
            companionStats.health += 1;
        }
    }

    public void SpendXpPointDmg()
    {
        if (companionStats.xpPoints > 0)
        {
            companionStats.xpPoints -= 1;
            companionStats.damage += 1;
        }
    }
    public void SpendXpPointDef()
    {
        if (companionStats.xpPoints > 0)
        {
            companionStats.xpPoints -= 1;
            companionStats.defense += 1;
        }
    }
    public void SpendXpPointAttackSpeed()
    {
        if (companionStats.xpPoints > 0)
        {
            companionStats.xpPoints -= 1;
            companionStats.attackSpeed -= 0.2f;
        }
    }

public void ShowTab(string tabName)
{
    if (tabName == "CreatureTab")
    {
        creatureTab.SetActive(true);
        runesTab.SetActive(false);
        itemsTab.SetActive(false);
        creatureTabButton.image.color = creatureTabButton.colors.selectedColor; // Set creature tab button to selected color
        runesTabButton.image.color = runesTabButton.colors.normalColor; // Set runes tab button to default
        itemsTabButton.image.color = itemsTabButton.colors.normalColor; // Set items tab button to default
    }
    else if (tabName == "RunesTab")
    {
        creatureTab.SetActive(false);
        runesTab.SetActive(true);
        itemsTab.SetActive(false);
        creatureTabButton.image.color = creatureTabButton.colors.normalColor; // Set creature tab button to default
        runesTabButton.image.color = runesTabButton.colors.selectedColor; // Set runes tab button to selected color
        itemsTabButton.image.color = itemsTabButton.colors.normalColor; // Set items tab button to default
    }
    else if (tabName == "ItemsTab")
    {
        creatureTab.SetActive(false);
        runesTab.SetActive(false);
        itemsTab.SetActive(true);
        creatureTabButton.image.color = creatureTabButton.colors.normalColor; // Set creature tab button to default
        runesTabButton.image.color = runesTabButton.colors.normalColor; // Set runes tab button to default
        itemsTabButton.image.color = itemsTabButton.colors.selectedColor; // Set items tab button to selected color
    }
}

    public void ShowObject(GameObject obj)
    {
        if(obj.activeSelf == false)
        {
            obj.SetActive(true);
        }
        else
        {
            obj.SetActive(false);
        }
    }

    public void ShowDeathMenu()
    {
        titleObject.enabled = false;
        deadMenu.SetActive(true);
        Time.timeScale = 0.001f;
    }

    public void ShowInfoText(string info)
    {
        infoText.text = info;
        anim.SetTrigger("showInfoText");
    }

    public void ShowUI()
    {
        if (!isInventoryOpen)
        {
            PlayerController.Instance.frozen = true;
            Inventory.SetActive(true);
            anim.enabled = false;
            titleObject.enabled = false;
            isInventoryOpen = true;
            Time.timeScale = 0.001f;
        }
        else
        {
            PlayerController.Instance.frozen = false;
            Inventory.SetActive(false);
            isInventoryOpen = false;
            anim.enabled = true;
            titleObject.enabled = true;
            Time.timeScale = 1;
        }
    }




    public void CloseAllMenus()
    {
        deadMenu.SetActive(false);
        if(isInventoryOpen)
        ShowUI();
    }
}