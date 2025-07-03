using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scenes {
        MainMenuScene,
        LoadingScene,
        GameScene
    }
    private static Scenes targetScene;

    public static void Load(Scenes scene) {
        Loader.targetScene = scene;
        SceneManager.LoadScene(Scenes.LoadingScene.ToString());
    }

    public static void LoaderCallback() {
        
        SceneManager.LoadScene(targetScene.ToString());
    }
}
