using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWindow : MonoBehaviour
{
    private const string PLAY_BUTTON = "PlayButton";
    private const string QUIT_BUTTON = "QuitButton";

    private void Awake()
    {
        transform.Find(PLAY_BUTTON).GetComponent<Button_UI>().ClickFunc = () =>
        {
            Loader.Load(Scene.MainScene);
        };


        transform.Find(QUIT_BUTTON).GetComponent<Button_UI>().ClickFunc = () =>
        {
            Application.Quit();
        };
    }
}
