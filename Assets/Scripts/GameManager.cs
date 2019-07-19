﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Make sure scenes are added to build settings!

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    public string mainMenuSceneName = "MainMenu";

    [Tooltip("First level at index 0")]
    public string[] levelSceneNames;
    public string[] tutorialSceneNames;
    public string[] currentSceneNames;

    string currentSceneName;
    int sceneIndex = 0;

    // The following is kind've crappy way to test this, but it works!
    public bool triggerMainMenuScene = false;
    public bool triggerRestartScene = false;
    public bool triggerNextScene = false;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    // This is only implemented for testing, it can be removed later
    private void Update()
    {
        if (triggerMainMenuScene)
        {
            loadMainMenu();
        }
        else if (triggerRestartScene)
        {
            restartScene();
        }
        else if (triggerNextScene)
        {
            nextScene();
        }

        triggerMainMenuScene = false;
        triggerRestartScene = false;
        triggerNextScene = false;
    }

    public void playTutorialScenes()
    {
        currentSceneNames = tutorialSceneNames;
        sceneIndex = 0;
        nextScene();
    }

    public void playGameScenes()
    {
        currentSceneNames = levelSceneNames;
        sceneIndex = 0;
        nextScene();
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void loadMainMenu()
    {
        StartCoroutine(LoadSceneInBackground(mainMenuSceneName));
        currentSceneName = mainMenuSceneName;
        sceneIndex = 0;
    }

    public void restartScene()
    {
        StartCoroutine(LoadSceneInBackground(currentSceneName));
    }

    public void nextScene()
    {
        // Heavily based on https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.LoadSceneAsync.html
        string sceneName = currentSceneNames[sceneIndex];
        StartCoroutine(LoadSceneInBackground(sceneName));
        sceneIndex = (sceneIndex + 1) % currentSceneNames.Length;
        currentSceneName = sceneName;
    }

    IEnumerator LoadSceneInBackground(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
