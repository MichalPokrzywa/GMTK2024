using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void LoadScene(GameScene scene)
    {
        LoadScene((int)scene);
    }
}

public enum GameScene
{
    Initialization = 0,
    Menu = 1,
    Main = 2,

    //Add another scene like this ^
}
