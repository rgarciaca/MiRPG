using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : MonoBehaviour
{
    public static DialogueWindow instance;

    public GameObject playerWindow;
    public GameObject npc1Window;
    public GameObject npc2Window;
    public GameObject npc3Window;



    private DialogueLine line;
    SpeakerType typeCharacter;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartDialogue(int index)
    {
        ChangeStateWindow(gameObject, true);
        DisableWindows();

        line = DialogueManager.instance.StartDialogue(index);
        ShowDialogueWindow();
    }

    public void NextDialogueLine()
    {
        DisableWindows();

        line = DialogueManager.instance.NextDialogueLine();

        if (line == null)
        {
            FinishDialogue();
        }
        else
        {
            ShowDialogueWindow();
        }
    }

    private void FinishDialogue()
    {
        DisableWindows();
        ChangeStateWindow(gameObject);
    }

    private void DisableWindows()
    {
        ChangeStateWindow(playerWindow);
        ChangeStateWindow(npc1Window);
        ChangeStateWindow(npc2Window);
        ChangeStateWindow(npc3Window);
    }

    public void ChangeStateWindow(GameObject window, bool show = false)
    {
        CanvasGroup cv = window.GetComponent<CanvasGroup>();
        cv.alpha = Convert.ToInt32(show);
        cv.blocksRaycasts = show;
        cv.interactable = show;
        Time.timeScale = Convert.ToInt32(!show);
    }

    private void ShowDialogueWindow()
    {
        typeCharacter = DialogueManager.instance.CurrentSpeakerType(line.characterName);

        if (typeCharacter == SpeakerType.Player)
        {
            ShowDialogueWindow(playerWindow);
        }

        if (typeCharacter == SpeakerType.NPC1)
        {
            ShowDialogueWindow(npc1Window);
        }

        if (typeCharacter == SpeakerType.NPC2)
        {
            ShowDialogueWindow(npc2Window);
        }

        if (typeCharacter == SpeakerType.NPC3)
        {
            ShowDialogueWindow(npc3Window);
        }
    }

    private void ShowDialogueWindow(GameObject window)
    {
        ChangeStateWindow(window, true);
        var imgs = window.GetComponentsInChildren<Image>();
        foreach (Image img in imgs)
        {
            if (img.gameObject.name == "Image")
            {
                if (window.name.Contains("Player"))
                {
                    img.sprite = GameManager.instance.sprPlayer;
                }
                else
                {
                    GameObject npc = GameObject.Find(line.characterName);
                    img.sprite = npc.GetComponent<SpriteRenderer>().sprite;
                }
            }
        }

        window.GetComponentsInChildren<Text>()[0].text = line.text;
        if (line.characterName.Contains("Player"))
            window.GetComponentsInChildren<Text>()[1].text = GameManager.instance.gameData.playerData.name;
        else
            window.GetComponentsInChildren<Text>()[1].text = GameObject.Find(line.characterName).GetComponent<NPC>().characterName;
    }
}

