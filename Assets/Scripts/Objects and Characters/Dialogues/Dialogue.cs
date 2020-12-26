using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Dialogue
{
    public int id;

    public string playerName = string.Empty;
    public string npc1Name = string.Empty;
    public string npc2Name = string.Empty;
    public string npc3Name = string.Empty;

    public string startsWithCharacter;

    public DialogueRewards rewards;
        
    public List<DialogueLine> dialogueLines;
}
