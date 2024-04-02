using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static int targetSceneInex;

    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }

    public static Scene targetScene;

    public static void Load(Scene targetSceneName)
    {
        targetScene = targetSceneName;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());

    }
}
