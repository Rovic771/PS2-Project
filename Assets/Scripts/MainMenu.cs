using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour //fait par Aksel
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable param;
    [SerializeField] private Selectable check;
    [SerializeField] private Selectable mainMenu;
    
    
    // là aussi faut dire à Aksel de faire gaffe aux conventions du projet
    public void newGame()
    {
        PlayerPrefs.SetFloat("checkpointX", 167.2f);
        // bah ptin, c'est précis
        PlayerPrefs.SetFloat("checkpointY", -0.5999985f);
        SceneManager.LoadScene("good assanblage 1");
    }

    public void quitGame()
    {
        Application.Quit();
        print("Quit Game");
    }
    
    public void jumpToParametre()
    {
        eventSystem.SetSelectedGameObject(param.gameObject);
    }
    
    public void jumpToCheckpoint()
    {
        eventSystem.SetSelectedGameObject(check.gameObject);
    }
    
    public void returntoMain()
    {
        eventSystem.SetSelectedGameObject(mainMenu.gameObject);
    }

    public void checkPoint1()
    {
        PlayerPrefs.SetFloat("checkpointX", 296.4f);
        PlayerPrefs.SetFloat("checkpointY", 1.85f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint2()
    {
        PlayerPrefs.SetFloat("checkpointX", 468.21f);
        PlayerPrefs.SetFloat("checkpointY", 63.92f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint3()
    {
        PlayerPrefs.SetFloat("checkpointX", 699.4f);
        PlayerPrefs.SetFloat("checkpointY", 74.48f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint4()
    {
        PlayerPrefs.SetFloat("checkpointX", 783.52f);
        PlayerPrefs.SetFloat("checkpointY", 70.56f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint5()
    {
        PlayerPrefs.SetFloat("checkpointX", 1016.22f);
        PlayerPrefs.SetFloat("checkpointY", 111.54f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint6()
    {
        PlayerPrefs.SetFloat("checkpointX", 1127.15f);
        PlayerPrefs.SetFloat("checkpointY", 133.52f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint7()
    {
        PlayerPrefs.SetFloat("checkpointX", 1290.58f);
        PlayerPrefs.SetFloat("checkpointY", 121.51f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint8()
    {
        PlayerPrefs.SetFloat("checkpointX", 1357.15f);
        PlayerPrefs.SetFloat("checkpointY", 136.45f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint9()
    {
        PlayerPrefs.SetFloat("checkpointX", 1449.07f);
        PlayerPrefs.SetFloat("checkpointY", 149.49f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
}

