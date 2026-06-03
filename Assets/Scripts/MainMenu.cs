using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject disque;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable param;
    //[SerializeField] private Selectable check;
    [SerializeField] private Selectable mainMenu;
    
    
    public void newGame()
    {
        SceneManager.LoadScene("good assanblage GD");
    }

    public void quitGame()
    {
        Application.Quit();
        print("Quit Game");
    }
    
    public void jumpToParametre()
    {
        RectTransform react = disque.GetComponent<RectTransform>();
        react.anchoredPosition = new Vector2(0, 22f);
        eventSystem.SetSelectedGameObject(param.gameObject);
    }
    
    //public void jumpToCheckpoint()
    //{
    //    eventSystem.SetSelectedGameObject(check.gameObject);
    //}
    
    public void returntoMain()
    {
        eventSystem.SetSelectedGameObject(mainMenu.gameObject);
        RectTransform react = disque.GetComponent<RectTransform>();
        react.anchoredPosition = new Vector2(-692.7f, 22f);
    }
    
}

