using System;
using System.Collections;
using System.Collections.Generic;

public enum ObjectiveType
{
    Collect,
    Slay,
    Location,
    Speak
}

[Serializable]
public class QuestObjectives
{
    public string name;
    public ObjectiveType Type;
    public int amount;  
}