using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private Selectable param;
    [SerializeField] private Selectable check;

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
    
    public void returntoMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }
}
