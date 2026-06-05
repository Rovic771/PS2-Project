using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable param;
    [SerializeField] private Selectable check;
    [SerializeField] private Selectable menu;
    

    public void resume()
    {
        Time.timeScale = 1f;
    }
    
    public void jumpToParametre()
    {
        eventSystem.SetSelectedGameObject(param.gameObject);
    }
    
    public void jumpToCheckpoint()
    {
        eventSystem.SetSelectedGameObject(check.gameObject);
    }
    
    public void jumpToMenu()
    {
        eventSystem.SetSelectedGameObject(menu.gameObject);
    }
    
    public void returntoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }
    
    public void checkPoint1()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("checkpointX", 296.4f);
        PlayerPrefs.SetFloat("checkpointY", 1.85f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint2()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("checkpointX", 468.21f);
        PlayerPrefs.SetFloat("checkpointY", 63.92f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint3()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("checkpointX", 699.4f);
        PlayerPrefs.SetFloat("checkpointY", 74.48f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint4()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("checkpointX", 783.52f);
        PlayerPrefs.SetFloat("checkpointY", 70.56f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint5()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("checkpointX", 1016.22f);
        PlayerPrefs.SetFloat("checkpointY", 111.54f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint6()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("checkpointX", 1127.15f);
        PlayerPrefs.SetFloat("checkpointY", 133.52f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint7()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("checkpointX", 1290.58f);
        PlayerPrefs.SetFloat("checkpointY", 121.51f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint8()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("checkpointX", 1357.15f);
        PlayerPrefs.SetFloat("checkpointY", 136.45f);
        SceneManager.LoadScene("good assanblage 1");
    }
    
    public void checkPoint9()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetFloat("checkpointX", 1449.07f);
        PlayerPrefs.SetFloat("checkpointY", 149.49f);
        SceneManager.LoadScene("good assanblage 1");
    }
}
