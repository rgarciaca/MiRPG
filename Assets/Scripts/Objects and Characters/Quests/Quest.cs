using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Quest
{
    public int id;

    public string internalTitle = string.Empty;
    public string title = string.Empty;
    public string description = string.Empty;
    public string descriptionInProgress = string.Empty;

    public string startsWithCharacter;
    public string endsWithCharacter;

    public int experienceLevel;
    public int requiredQuestID;

    public List<QuestObjectives> objectives;

    //public DialogueRewards rewards;

    //public List<DialogueLine> dialogueLines;
}
