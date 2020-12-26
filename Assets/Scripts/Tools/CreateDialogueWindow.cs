using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateDialogueWindow : EditorWindow
{
    List<String> sceneOptions;
    List<String> charactersNPC;
    Dialogues dialogues;
    int indexSelectedDialogue = -1;
    int indexSelectedScene = -1;
    int indexSelectedCharacter = -1;
    public Dialogue currentDialogue;

[MenuItem("RPG Tools/Create Dialogue")]
    public static void ShowWindow()
    {
        GetWindow<CreateDialogueWindow>(false, "Create Dialogue", true);
    }

    private void Awake()
    {
        dialogues = new Dialogues();
        currentDialogue = new Dialogue();
    }

    private void OnGUI()
    {
        LoadSceneOptions();
        LoadCharactersNPC();

        GUIStyle styleGroup = new GUIStyle(GUI.skin.button);
        styleGroup.border = new RectOffset(1, 1, 1, 1);
        //styleLoad.margin = new UnityEngine.RectOffset(10, 0, 10, 0);

        GUIStyle styleLoad = new GUIStyle(GUI.skin.button);
        styleLoad.border = new RectOffset(1, 1, 1, 1);
        styleLoad.margin = new UnityEngine.RectOffset(10, 0, 10, 0);

        GUIStyle styleSave = new GUIStyle(GUI.skin.button);
        styleSave.border = new RectOffset(1, 1, 1, 1);
        styleSave.margin = new UnityEngine.RectOffset(5, 0, 10, 0);

        GUIStyle styleNew = new GUIStyle(GUI.skin.button);
        styleNew.border = new RectOffset(1, 1, 1, 1);
        styleNew.margin = new UnityEngine.RectOffset(10, 0, 5, 0);

        
        GUI.BeginGroup(new Rect(10, 10, 195, 60), styleGroup);
        GUILayout.BeginArea(new Rect(10, 10, 175, 40));
        GUILayout.Label("Select scene:");
        indexSelectedScene = EditorGUILayout.Popup(indexSelectedScene, sceneOptions.ToArray(), GUILayout.Width(175));
        if (indexSelectedScene > -1)
        {
            dialogues.sceneName = sceneOptions[indexSelectedScene];
        }
        GUILayout.EndArea();
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(10, 80, 195, 65), styleGroup);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load", styleLoad, GUILayout.Width(85)))
        {
            if (indexSelectedScene > -1)
            {
                Debug.Log("Boton Load presionado");
                LoadDialogues();
            }
            else
            {
                Debug.Log("Boton Load presionado sin escena");
                //GUILayout.Label("Yo must select a scene.");
            }
        }
        if (GUILayout.Button("Save", styleSave, GUILayout.Width(85)))
        {
            Debug.Log("Boton Save presionado");
            SaveDialogue();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New entry", styleNew, GUILayout.Width(175)))
        {
            ResetDataDialogue();
        }
        GUILayout.EndHorizontal();
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(10, 155, 195, 380), styleGroup);
        GUILayout.BeginArea(new Rect(10, 10, 175, 430));
        GUIStyle styleDial = new GUIStyle(GUI.skin.button);
        styleDial.border = new RectOffset(1, 1, 1, 1);
        styleDial.margin = new UnityEngine.RectOffset(10, 0, 10, 0);
        if (dialogues.sceneDialogues != null)
        {
            for (int i = 0; i < dialogues.sceneDialogues.Count; i++)
            {
                if (i > 0)
                {
                    styleDial.margin = new UnityEngine.RectOffset(10, 0, 0, 0);
                }

                if (GUILayout.Button("Dialogue " + i, styleDial, GUILayout.Width(175)))
                {
                    Debug.Log("Selected dialogue: Dialogue " + i);
                    LoadDataDialogueSelected(i);
                }
            }
        }
        GUILayout.EndArea();
        GUI.EndGroup();

        GUIStyle styleText = new GUIStyle(GUI.skin.textField);
        styleText.margin = new UnityEngine.RectOffset(10, 10, 0, 5);

        GUIStyle styleLabel = new GUIStyle(GUI.skin.label);
        styleLabel.margin = new UnityEngine.RectOffset(10, 10, 0, 15);        

        GUI.BeginGroup(new Rect(215, 10, 500, 525), styleGroup);
        GUILayout.BeginArea(new Rect(10, 10, 480, 505));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Dialogue ID", styleLabel, GUILayout.Width(90));
        if (currentDialogue.id == 0)
        {
            styleLabel.fontStyle = FontStyle.Bold;
            styleLabel.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label("", styleLabel);
        }
        else
        {
            styleLabel.fontStyle = FontStyle.Bold;
            styleLabel.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label(currentDialogue.id.ToString(), styleLabel);
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.TextField("Player Name", currentDialogue.playerName, styleText, GUILayout.Width(470));
        EditorGUILayout.TextField("NPC 1 Name", currentDialogue.npc1Name, styleText, GUILayout.Width(470));
        EditorGUILayout.TextField("NPC 2 Name", currentDialogue.npc2Name, styleText, GUILayout.Width(470));
        EditorGUILayout.TextField("NPC 3 Name", currentDialogue.npc3Name, styleText, GUILayout.Width(470));
         

        GUIStyle stylePopup = new GUIStyle();
        stylePopup.margin = new UnityEngine.RectOffset(5, 10, 10, 10);

        GUILayout.BeginHorizontal(stylePopup);
        GUILayout.Label("Select the character to start the dialogue", GUILayout.Width(240));
        indexSelectedCharacter = EditorGUILayout.Popup(indexSelectedCharacter, charactersNPC.ToArray(), GUILayout.Width(225));
        if (indexSelectedCharacter > -1)
        {
            currentDialogue.startsWithCharacter = charactersNPC[indexSelectedCharacter];
        }
        GUILayout.EndHorizontal();


        // "target" can be any class derrived from ScriptableObject 
        // (could be EditorWindow, MonoBehaviour, etc)
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty dialogueProp = so.FindProperty("currentDialogue.dialogueLines");

        EditorGUILayout.PropertyField(dialogueProp, true, GUILayout.Width(480)); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties

        GUILayout.EndArea();
        GUI.EndGroup();
    }

    private void LoadCharactersNPC()
    {
        charactersNPC = new List<string>();

        var NPCs = GameObject.FindGameObjectsWithTag("NPC");
        foreach (GameObject npc in NPCs)
        {
            charactersNPC.Add(npc.name);
        }
    }

    private void LoadSceneOptions()
    {
        sceneOptions = new List<string>();

        var scenes = AssetDatabase.FindAssets("t:Scene");
        foreach (string guid in scenes)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("Assets/Scenes") && !path.Contains("CreateNewGame"))
            {
                string[] items = path.Split(new String[1] { "/" }, StringSplitOptions.None);
                sceneOptions.Add(items[items.Length - 1].Replace(".unity", ""));
            }
        }
    }

    private void LoadDialogues()
    {
        string filePath = Application.dataPath + "/Resources/Dialogues/" + dialogues.sceneName + "_dialogues.json";
        string json = System.IO.File.ReadAllText(filePath);
        if (!string.IsNullOrEmpty(json))
            dialogues = JsonUtility.FromJson<Dialogues>(json);

        //dialogues.sceneName = sceneOptions[indexSelectedScene];
    }

    private void SaveDialogue()
    {
        if (indexSelectedDialogue == -1)
            dialogues.sceneDialogues.Add(currentDialogue);
        else
            dialogues.sceneDialogues[indexSelectedDialogue] = currentDialogue;

        string filePath = Application.dataPath + "/Resources/Dialogues/" + dialogues.sceneName + "_dialogues.json";
        string json = JsonUtility.ToJson(dialogues);
        System.IO.File.WriteAllText(filePath, json);
    }

    private void LoadDataDialogueSelected(int index)
    {
        indexSelectedDialogue = index;
        currentDialogue = dialogues.sceneDialogues[index];
        indexSelectedCharacter = charactersNPC.IndexOf(currentDialogue.startsWithCharacter);
    }

    private void ResetDataDialogue()
    {
        indexSelectedDialogue = -1;
        indexSelectedCharacter = -1;
        currentDialogue = new Dialogue();
        currentDialogue.id = dialogues.sceneDialogues.Count + 1;
    }
}