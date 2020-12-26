using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    public static StatsPanel instance;

    public Image expBar;
    public Image healthBar;
    public Text coinsText;
    public Text playerNameText;
    public Text levelText;
    public Text attributePointsText;
    public GameObject statsPanel;
    public CanvasGroup infoPanels;
    public CanvasGroup inventory;

     private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateInfo();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateInfo()
    {
        UpdateStatsPanel();
        UpdateHealthBar();
        UpdateExperienceBar();
        UpdateCoins();
    }

    private void UpdateStatsPanel()
    {
        playerNameText.text = PlayerAttributes.instance.playerData.name;
        levelText.text = "Level: " + PlayerAttributes.instance.playerData.stats.Level.ToString();

        attributePointsText.text = PlayerAttributes.instance.playerData.stats.Points.ToString();

        Text[] texts = statsPanel.GetComponentsInChildren<Text>();
        foreach (Text textComponent in texts)
        {
            if (!textComponent.name.Contains("Label"))
            {
                if (textComponent.name.Contains("Health"))
                {
                    textComponent.text = PlayerAttributes.instance.CurrentHealth.ToString();
                }
                if (textComponent.name.Contains("Experience"))
                {
                    textComponent.text = PlayerAttributes.instance.experience.ToString();
                }
                if (textComponent.name.Contains("Strength"))
                {
                    textComponent.text = PlayerAttributes.instance.totalStrength.ToString();
                }
                if (textComponent.name.Contains("Armor"))
                {
                    textComponent.text = PlayerAttributes.instance.totalArmor.ToString();
                }
            }
        }

        SetVisibilityAttributeButtons();
    }

    public void UpdateHealthBar()
    {
        healthBar.fillAmount = (float)PlayerAttributes.instance.CurrentHealth / PlayerAttributes.instance.totalHealth;
    }

    public void UpdateExperienceBar()
    {
        expBar.fillAmount = PlayerAttributes.instance.rateCurrentExperienceLevel;
    }

    public void UpdateCoins()
    {
        coinsText.text = PlayerAttributes.instance.playerData.coins.ToString();
    }

    private void SetVisibilityAttributeButtons()
    {
        Button[] buttons = statsPanel.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive((PlayerAttributes.instance.playerData.stats.Points > 0));
        }
    }
    public void ChangeInfoPanelsState()
    {
        bool isInfoPanelsVisible = Convert.ToBoolean(infoPanels.alpha);
        infoPanels.alpha = Convert.ToInt32(!isInfoPanelsVisible);
        infoPanels.blocksRaycasts = !isInfoPanelsVisible;
        infoPanels.interactable = !isInfoPanelsVisible;
        Time.timeScale = Convert.ToInt32(isInfoPanelsVisible);

        if (infoPanels.alpha == 0 && inventory.alpha == 1)
        {
            ChangeInventoryState();
        }
    }

    public void ChangeInventoryState()
    {
        bool isInventoryVisible = Convert.ToBoolean(inventory.alpha);
        inventory.alpha = Convert.ToInt32(!isInventoryVisible);
        inventory.blocksRaycasts = !isInventoryVisible;
        inventory.interactable = !isInventoryVisible;
    }

    public void UpdateEquipmentValues(Equipment equipment)
    {
        //List<Equipment> equipment = EquipmentPanel.instance.items;
        //foreach (Equipment item in equipment)
        //{
            PlayerAttributes.instance.playerData.stats.Health.Modifiers += equipment.healthChange;
            PlayerAttributes.instance.playerData.stats.Armor.Modifiers += equipment.armorChange;
            PlayerAttributes.instance.playerData.stats.Strength.Modifiers += equipment.strengthChange;
        //}

        UpdateInfo();
    }

    public void RemoveEquipment(Equipment equipment)
    {
        PlayerAttributes.instance.playerData.stats.Health.Modifiers -= equipment.healthChange;
        PlayerAttributes.instance.playerData.stats.Armor.Modifiers -= equipment.armorChange;
        PlayerAttributes.instance.playerData.stats.Strength.Modifiers -= equipment.strengthChange;

        UpdateInfo();
    }
}
