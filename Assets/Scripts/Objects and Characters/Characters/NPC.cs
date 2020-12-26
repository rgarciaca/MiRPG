using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactive
{
    public string characterName;
    public GameObject dialogueIndicator;

    private bool haveDialogue;
    private int dialogueIndex;
    //public void Hablar()
    //{
    //    DialogueWindow.instance.StartDialogue(DialogueManager.instance.dialogues.gameDialogues[0]);
    //}

    private void Awake()
    {
        haveDialogue = false;
        dialogueIndicator.GetComponent<Renderer>().sortingLayerID = transform.GetComponent<Renderer>().sortingLayerID;
        //GetComponent<Renderer>().sortingLayerName = "Indicators";
    }

    private void Update()
    {
        ActiveDialogue();
    }

    public override void OnMouseEnter()
    {
        if (haveDialogue)
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    public override void Interaction()
    {
        if (haveDialogue)
            DialogueWindow.instance.StartDialogue(dialogueIndex);
    }

    private void ActiveDialogue()
    {

        dialogueIndex = DialogueManager.instance.NPCActiveDialogueIndex(gameObject.name);
        if (dialogueIndex > -1)
        {
            haveDialogue = true;
            dialogueIndicator.SetActive(true);
        }
        else
        {
            haveDialogue = false;
            dialogueIndicator.SetActive(false);
        }

    }

}
