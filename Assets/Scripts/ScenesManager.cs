using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;
    public bool isLevel;
    public SceneData currentScene;
    public int sceneIndex = -1;

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
        if (isLevel)
        {
            LoadSceneData();
        }
    }

    // Start is called before the first frame update
    public void LoadNextScene()
    {
        int escenaActualIndice = SceneManager.GetActiveScene().buildIndex;
        int escenaSiguienteIndice = escenaActualIndice + 1;

        SceneManager.LoadScene(escenaSiguienteIndice);
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    private void LoadSceneData()
    {
        if (GameManager.instance.gameData.scenesData == null)
            GameManager.instance.gameData.scenesData = new List<SceneData>();

        currentScene = null;
        sceneIndex = -1;
        if (GameManager.instance.gameData.scenesData != null && GameManager.instance.gameData.scenesData.Count > 0)
        {
            foreach (SceneData item in GameManager.instance.gameData.scenesData)
            {
                if (item.sceneName == SceneManager.GetActiveScene().name)
                {
                    currentScene = item;
                    sceneIndex = GameManager.instance.gameData.scenesData.IndexOf(item);
                }
            }
        }
        
        if (currentScene == null)
        {
            sceneIndex = -1;
            currentScene = new SceneData();
            currentScene.sceneName = SceneManager.GetActiveScene().name;
        }

        currentScene = LoadDialoguesData(currentScene);

        if (sceneIndex == -1)
        {
            GameManager.instance.gameData.scenesData.Add(currentScene);
            sceneIndex = GameManager.instance.gameData.scenesData.Count - 1;
        }
        else
            GameManager.instance.gameData.scenesData[sceneIndex] = currentScene;


    }

    private SceneData LoadDialoguesData(SceneData scene)
    {
        SceneData resScenedata = scene;

        Dialogues sceneDials = DialogueManager.instance.dialogues;
        foreach (Dialogue sceneDial in sceneDials.sceneDialogues)
        {
            DialogueData item = null;
            if (resScenedata.dialoguesData != null && resScenedata.dialoguesData.Count > 0)
            {
                item = resScenedata.dialoguesData.Where(di => di.id.Equals(sceneDial.id)).FirstOrDefault();
            }
            else
            {
                resScenedata.dialoguesData = new List<DialogueData>();
            }

            if (item == null)
            {
                DialogueData dialData = new DialogueData();
                dialData.id = sceneDial.id;
                dialData.startsWithCharacter = sceneDial.startsWithCharacter;
                dialData.isActive = DialogueManager.instance.IsDialogueActive(sceneDial.id);

                resScenedata.dialoguesData.Add(dialData);
            }

        }

        return resScenedata;
    }
}
