using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeakerType
{
    Player,
    NPC1,
    NPC2,
    NPC3
}


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public Dialogues dialogues;

    private Dialogue currentDialogue;
    private int lineDialogueIndex;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        dialogues = new Dialogues();

        string filePath = Application.dataPath + "/Resources/Dialogues/" + ScenesManager.instance.GetSceneName() + "_dialogues.json";
        string json = System.IO.File.ReadAllText(filePath);
        if (!string.IsNullOrEmpty(json))
            dialogues = JsonUtility.FromJson<Dialogues>(json);
    }

    public DialogueLine StartDialogue(int index)
    {
        currentDialogue = dialogues.sceneDialogues[index];
        lineDialogueIndex = 0;

        return currentDialogue.dialogueLines[lineDialogueIndex];
    }

    public DialogueLine NextDialogueLine()
    {
        lineDialogueIndex++;
        if (currentDialogue.dialogueLines.Count <= lineDialogueIndex)
        {
            var item = ScenesManager.instance.currentScene.dialoguesData.Where(di => di.id.Equals(currentDialogue.id)).FirstOrDefault();
            ScenesManager.instance.currentScene.dialoguesData[ScenesManager.instance.currentScene.dialoguesData.IndexOf(item)].isActive = false;
            ScenesManager.instance.currentScene.dialoguesData[ScenesManager.instance.currentScene.dialoguesData.IndexOf(item)].isCompleted = true;
                
            return null;
        }
        else
        {
            return currentDialogue.dialogueLines[lineDialogueIndex];
        }
    }

    public SpeakerType CurrentSpeakerType(string name)
    {
        return (SpeakerType)Enum.Parse(typeof(SpeakerType), name);
    }

    public int NPCActiveDialogueIndex(string name)
    {
        var item = ScenesManager.instance.currentScene.dialoguesData.Where(di => di.startsWithCharacter == name && di.isActive && !di.isCompleted).FirstOrDefault();
        if (item != null)
        {
            return ScenesManager.instance.currentScene.dialoguesData.IndexOf(item);
        }

        return -1;
    }

    public bool IsDialogueActive(int id)
    {
        return true;
    }


}
