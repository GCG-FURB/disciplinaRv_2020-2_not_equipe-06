using UnityEngine.SceneManagement;

public static class Loader 
{
    private static Scene _targetScene;

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(Scene.Loading.ToString());

        _targetScene = scene;
    }

    public static void LoadTargetScene()
    {
        SceneManager.LoadScene(_targetScene.ToString());
    }
}
