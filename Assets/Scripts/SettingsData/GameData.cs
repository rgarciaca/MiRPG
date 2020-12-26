using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public PlayerData playerData;
    public List<SceneData> scenesData;
}

public enum Gender
{
    Male,
    Female
}
