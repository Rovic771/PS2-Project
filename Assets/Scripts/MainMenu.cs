using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject disque;
    
    public void newGame()
    {
        SceneManager.LoadScene("good assanblage GD");
    }

    public void quitGame()
    {
        Application.Quit();
        print("Quit Game");
    }

    public void parametre()
    {
        RectTransform react = disque.GetComponent<RectTransform>();
        react.Translate(0f,22,0f);
    }
}
