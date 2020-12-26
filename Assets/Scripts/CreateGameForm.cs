using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class CreateGameForm : MonoBehaviour
{
    public Image playerImage;
    public Slider sliderTraits;

    private Gender currentGender;
    private int currentTraits;

    // Start is called before the first frame update
    void Start()
    {
        currentGender = Gender.Male;
        currentTraits = 1;

        sliderTraits.onValueChanged.AddListener(delegate { OnValueChangeTraits(); });

        DrawPlayerImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        GameData settings = new GameData();
        settings.playerData = new PlayerData();
        settings.playerData.name = "";
        settings.playerData.gender = currentGender;
        settings.playerData.traits  = currentTraits;
        settings.playerData.speed = InitialValues.PLAYER_INITIAL_SPEED;
        settings.playerData.coins = InitialValues.PLAYER_INITIAL_COINS;

        settings.playerData.stats = new Stats();
        settings.playerData.stats.Level = InitialValues.PLAYER_INITIAL_LEVEL;

        settings.playerData.stats.Experience = new Stat();
        settings.playerData.stats.Experience.Base = InitialValues.PLAYER_INITIAL_EXPERIENCE;
        settings.playerData.stats.Experience.Current = InitialValues.PLAYER_INITIAL_EXPERIENCE;

        settings.playerData.stats.Armor = new Stat();
        settings.playerData.stats.Armor.Base = InitialValues.PLAYER_INITIAL_ARMOR;
        settings.playerData.stats.Armor.Current = InitialValues.PLAYER_INITIAL_ARMOR;

        settings.playerData.stats.Strength = new Stat();
        settings.playerData.stats.Strength.Base = InitialValues.PLAYER_INITIAL_STRENGTH;
        settings.playerData.stats.Strength.Current = InitialValues.PLAYER_INITIAL_STRENGTH;

        settings.playerData.stats.Health = new Stat();
        settings.playerData.stats.Health.Base = InitialValues.PLAYER_INITIAL_HEALTH;
        settings.playerData.stats.Health.Current = InitialValues.PLAYER_INITIAL_HEALTH;
        settings.playerData.stats.Health.IncreaseNextLevel = InitialValues.PLAYER_HEALTH_INCREMENT_BY_LEVEL;

        settings.playerData.stats.Points = InitialValues.PLAYER_INITIAL_ATTRIBUTE_POINTS;

        //settings.scenesData = new List<SceneData>();

        string json = JsonUtility.ToJson(settings);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/GameData.json", json);

        ScenesManager.instance.LoadNextScene();
    }

    public void OnChangeMaleGender(bool value)
    {
        currentGender = Gender.Male;
        DrawPlayerImage();
    }

    public void OnChangeFemaleGender(bool value)
    {
        currentGender = Gender.Female;
        DrawPlayerImage();
    }

    public void OnValueChangeTraits()
    {
        currentTraits = (int)sliderTraits.value;
        DrawPlayerImage();
    }

    private void DrawPlayerImage()
    {
        GameObject jugadorGO;
        string path = "Assets/Prefabs/Player/Player" + currentGender.ToString() + currentTraits.ToString() + ".prefab";

        jugadorGO = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        if (jugadorGO != null)
        {
            SpriteRenderer spr = jugadorGO.GetComponent<SpriteRenderer>();
            playerImage.sprite = spr.sprite;
        }
    }
}
