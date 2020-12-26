using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public static PlayerAttributes instance;

    public  PlayerData playerData;

    public float rateCurrentExperienceLevel;
    public int totalHealth { get { return playerData.stats.Health.Base + playerData.stats.Health.Modifiers; } }
    public int totalArmor { get { return playerData.stats.Armor.Base + playerData.stats.Armor.Modifiers; } }
    public int totalStrength { get { return playerData.stats.Strength.Base + playerData.stats.Strength.Modifiers; } }

    public int CurrentHealth
    {
        get
        {
            return playerData.stats.Health.Current;
        }

        set
        {
            if (value > 0 && value <= totalHealth)
            {
                playerData.stats.Health.Current = value;
            }
            else if (value > totalHealth)
            {
                playerData.stats.Health.Current = totalHealth;
            }
            else
            {
                playerData.stats.Health.Current = 0;
            }
        }
    }

    public int experience
    {
        get { return playerData.stats.Experience.Current; }
        set
        {
            playerData.stats.Experience.Current = value;

            if (playerData.stats.Level > 1)
            {
                rateCurrentExperienceLevel = (float)(experience - AccumulatedExperienceCurve(playerData.stats.Level)) / (float)playerData.stats.Experience.IncreaseNextLevel;
            }
            else
            {
                rateCurrentExperienceLevel = (float)(playerData.stats.Experience.Current) / playerData.stats.Experience.IncreaseNextLevel;
            }

            while (rateCurrentExperienceLevel >= 1)
            {
                LevelUp();
            }

            StatsPanel.instance.UpdateInfo();
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        playerData = GameManager.instance.gameData.playerData;

        playerData.stats.Level = 1;
        playerData.stats.Experience.IncreaseNextLevel = ExperienceCurve(playerData.stats.Level);

        StatsPanel.instance.UpdateInfo();
    }

    private int ExperienceCurve(int level)
    {
        float functionExp = (Mathf.Log(level, 3f) + 10);
        int exp = Mathf.CeilToInt(functionExp);

        return exp;
    }

    private int AccumulatedExperienceCurve(int nivel)
    {
        int exp = 0;
        for (int i = 1; i < nivel; i++)
        {
            exp += ExperienceCurve(i);
        }

        return exp;
    }

    private void LevelUp()
    {
        playerData.stats.Level++;
        SetupNextLevel();
        rateCurrentExperienceLevel = (float)(experience - AccumulatedExperienceCurve(playerData.stats.Level)) / playerData.stats.Experience.IncreaseNextLevel;
    }

     private void SetupNextLevel()
    {
        playerData.stats.Points++;
        playerData.stats.Experience.IncreaseNextLevel = ExperienceCurve(playerData.stats.Level);

        StatsPanel.instance.UpdateInfo();
    }

    public void ModifyAttibutePoints(int quantity)
    {
        playerData.stats.Points += quantity;

        StatsPanel.instance.UpdateInfo();
    }

    public void ModifyCurrentHealth(int cantidad)
    {
        CurrentHealth += cantidad;
        StatsPanel.instance.UpdateInfo();
    }

    public void ModifyBaseHealth(int quantity)
    {
        playerData.stats.Health.Base += quantity;
        StatsPanel.instance.UpdateInfo();
    }

    public void ModifyBaseArmor(int quantity)
    {
        playerData.stats.Armor.Base += quantity;
        StatsPanel.instance.UpdateInfo();
    }

    public void ModifyBaseStrength(int quantity)
    {
        playerData.stats.Strength.Base += quantity;
        StatsPanel.instance.UpdateInfo();
    }
}
