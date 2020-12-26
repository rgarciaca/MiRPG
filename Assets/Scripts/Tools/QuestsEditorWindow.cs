using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class QuestsEditorWindow : EditorWindow
{
    List<string> sceneOptions;
    List<string> charactersNPC;
    List<string> questsPopup;

    Quests quests;
    int indexSelectedDialogue = -1;
    int indexSelectedScene = -1;
    int indexStartCharacter = -1;
    int indexEndCharacter = -1;
    int requiredQuestID = -1;
    Vector2 scrollPos;

    string searchString = "";

    public Quest currentQuest;

    [MenuItem("RPG Tools/Create Quests")]
    public static void ShowWindow()
    {
        GetWindow<QuestsEditorWindow>(false, "Create Quests", true);
    }

    private void Awake()
    {
        quests = new Quests();
        quests.sceneQuests = new List<Quest>();
        currentQuest = new Quest();
        currentQuest.objectives = new List<QuestObjectives>();
    }

    private void OnGUI() 
    {
        LoadSceneOptions();
        LoadCharactersNPC();
        LoadQuestsPopup();

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
            quests.sceneName = sceneOptions[indexSelectedScene];
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
                LoadQuests();
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
            //SaveDialogue();
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
        GUIStyle styleQuest = new GUIStyle(GUI.skin.button);
        styleQuest.border = new RectOffset(1, 1, 1, 1);
        styleQuest.margin = new UnityEngine.RectOffset(10, 0, 10, 0);
        if (quests.sceneQuests != null)
        {
            for (int i = 0; i < quests.sceneQuests.Count; i++)
            {
                if (i > 0)
                {
                    styleQuest.margin = new UnityEngine.RectOffset(10, 0, 0, 0);
                }

                if (GUILayout.Button("Quest " + i, styleQuest, GUILayout.Width(175)))
                {
                    Debug.Log("Selected quest: Quest " + i);
                    LoadDataQuestSelected(i);
                }
            }
        }
        GUILayout.EndArea();
        GUI.EndGroup();


        GUIStyle styleText = new GUIStyle(GUI.skin.textField);
        styleText.margin = new UnityEngine.RectOffset(10, 10, 0, 5);

        GUIStyle styleLabel = new GUIStyle(GUI.skin.label);
        styleLabel.margin = new UnityEngine.RectOffset(10, 10, 0, 5);

        GUI.BeginGroup(new Rect(215, 10, 700, 85), styleGroup);
        GUILayout.BeginArea(new Rect(10, 10, 680, 65));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Quest ID", styleLabel, GUILayout.Width(140));
        if (currentQuest .id == 0)
        {
            styleLabel.fontStyle = FontStyle.Bold;
            styleLabel.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label("", styleLabel);
        }
        else
        {
            styleLabel.fontStyle = FontStyle.Bold;
            styleLabel.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label(currentQuest.id.ToString(), styleLabel);
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.TextField("Internal Title", currentQuest.internalTitle, styleText, GUILayout.Width(670));
        EditorGUILayout.TextField("Title", currentQuest.title, styleText, GUILayout.Width(670));

        GUILayout.EndArea();
        GUI.EndGroup();

        GUIStyle styleTextArea = new GUIStyle(GUI.skin.textArea);
        styleTextArea.margin = new UnityEngine.RectOffset(10, 10, 0, 5);

        GUI.BeginGroup(new Rect(215, 105, 345, 200), styleGroup);
        GUILayout.BeginArea(new Rect(10, 10, 325, 180));

        styleLabel.fontStyle = FontStyle.Normal;
        GUILayout.Label("Description", styleLabel, GUILayout.Width(315));
        EditorGUILayout.TextArea(currentQuest.description, styleTextArea, GUILayout.Width(325), GUILayout.Height(95));
        GUILayout.Label("Description In Progress", styleLabel, GUILayout.Width(315));
        EditorGUILayout.TextArea(currentQuest.descriptionInProgress, styleTextArea, GUILayout.Width(325), GUILayout.Height(35));

        GUILayout.EndArea();
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(215, 315, 345, 65), styleGroup);
        GUILayout.BeginArea(new Rect(10, 10, 325, 45));

        GUIStyle stylePopup = new GUIStyle();
        stylePopup.margin = new UnityEngine.RectOffset(0, 10, 0, 5);

        styleLabel = new GUIStyle(GUI.skin.label);
        styleLabel.margin = new UnityEngine.RectOffset(0, 10, 0, 5);

        GUILayout.BeginHorizontal(stylePopup);
        GUILayout.Label("Start character", styleLabel, GUILayout.Width(90));
        indexStartCharacter = EditorGUILayout.Popup(indexStartCharacter, charactersNPC.ToArray(), GUILayout.Width(225));
        if (indexStartCharacter > -1)
        {
            currentQuest.startsWithCharacter = charactersNPC[indexStartCharacter];
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("End character", styleLabel, GUILayout.Width(90));
        indexEndCharacter = EditorGUILayout.Popup(indexEndCharacter, charactersNPC.ToArray(), GUILayout.Width(225));
        if (indexEndCharacter > -1)
        {
            currentQuest.endsWithCharacter = charactersNPC[indexEndCharacter];
        }
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(215, 390, 345, 90), styleGroup);
        GUILayout.BeginArea(new Rect(10, 10, 325, 70));

        styleText = new GUIStyle(GUI.skin.textField);
        styleText.margin = new UnityEngine.RectOffset(0, 10, 0, 5);
        EditorGUILayout.TextField("Experience Level", currentQuest.experienceLevel.ToString(), styleText, GUILayout.Width(325));

        styleLabel = new GUIStyle(GUI.skin.label);
        styleLabel.margin = new UnityEngine.RectOffset(0, 10, 10, 5);
        GUILayout.Label("Required quest", styleLabel, GUILayout.Width(325));
        requiredQuestID = EditorGUILayout.Popup(requiredQuestID, questsPopup.ToArray(), GUILayout.Width(320));
        if (requiredQuestID > -1)
        {
            currentQuest.requiredQuestID = quests.sceneQuests[requiredQuestID].id;
        }
        GUILayout.EndArea();
        GUI.EndGroup();

        GUI.BeginGroup(new Rect(570, 105, 345, 220), styleGroup);
        GUILayout.BeginArea(new Rect(10, 10, 325, 200));

        GUILayout.BeginHorizontal();
        GUILayout.Label("Objectives", styleLabel, GUILayout.Width(220));
        
        if (GUILayout.Button("Add", styleSave, GUILayout.Width(85)))
        {
            Debug.Log("Boton Add presionado");
            currentQuest.objectives.Add(new QuestObjectives());
            //SaveDialogue();
        }
        GUILayout.EndHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Width(325), GUILayout.Height(10000));
        EditorGUILayout.BeginVertical(GUILayout.Width(280), GUILayout.Height(1000));
        
        LoadObjectives();

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        GUILayout.EndArea();
        GUI.EndGroup();



        //GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
        //searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
        //if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
        //{
        //    // Remove focus if cleared
        //    searchString = "";
        //    GUI.FocusControl(null);
        //}
        //GUILayout.EndHorizontal();

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


    private void LoadQuestsPopup()
    {
       questsPopup = new List<string>();

       foreach (Quest item in quests.sceneQuests)
       {
            questsPopup.Add(item.title);
       }
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

    private void LoadObjectives()
    {
        int index = 0;

        foreach (QuestObjectives obj in currentQuest.objectives)
        {
            DrawObjective(obj, index);
            index++;
        }
    }

    private void DrawObjective(QuestObjectives obj, int index)
    {
        int height = 30;

        GUIStyle styleArea = new GUIStyle(GUI.skin.button);
        styleArea.border = new RectOffset(1, 1, 1, 1);

        GUIStyle styleLabel = new GUIStyle(GUI.skin.label);
        styleLabel.margin = new UnityEngine.RectOffset(0, 10, 10, 5);

        //GUI.BeginGroup(new Rect(10, 40 + (index * height), 315, height), styleArea);

        GUILayout.BeginArea(new Rect(0, 40 + (index * height), 280, height), styleArea);


        GUILayout.Label("Objective " + index, styleLabel, GUILayout.Width(200));

        GUILayout.EndArea();

        //GUI.EndGroup();

    }

    private void LoadQuests()
    {
        string filePath = Application.dataPath + "/Resources/Quests/" + quests.sceneName + "_quests.json";
        string json = System.IO.File.ReadAllText(filePath);
        if (!string.IsNullOrEmpty(json))
            quests = JsonUtility.FromJson<Quests>(json);

        //dialogues.sceneName = sceneOptions[indexSelectedScene];
    }

    private void SaveQuests()
    {
        if (indexSelectedDialogue == -1)
            quests.sceneQuests.Add(currentQuest);
        else
            quests.sceneQuests[indexSelectedDialogue] = currentQuest;

        string filePath = Application.dataPath + "/Resources/Dialogues/" + quests.sceneName + "_dialogues.json";
        string json = JsonUtility.ToJson(quests);
        System.IO.File.WriteAllText(filePath, json);
    }


    private void LoadDataQuestSelected(int index)
    {
        indexSelectedDialogue = index;
        currentQuest = quests.sceneQuests[index];
        //indexSelectedCharacter = charactersNPC.IndexOf(currentDialogue.startsWithCharacter);
    }

    private void ResetDataDialogue()
    {
        indexSelectedDialogue = -1;
        //indexSelectedCharacter = -1;
        currentQuest = new Quest();
        currentQuest.id = quests.sceneQuests.Count + 1;
        currentQuest.objectives = new List<QuestObjectives>();
    }
}
