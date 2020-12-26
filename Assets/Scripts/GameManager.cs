using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public GameData gameData;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Sprite sprPlayer;

    public Texture2D cursorDefault;

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/GameData.json");
        gameData = JsonUtility.FromJson<GameData>(json);

        SetDefaultCursor();
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Start()
    {
    }
}
