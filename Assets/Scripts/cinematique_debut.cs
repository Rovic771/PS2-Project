using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class cinematique_debut : MonoBehaviour
{
    public float time;
    [SerializeField] List<CanvasGroup> images =  new List<CanvasGroup>();
    public int current = 0;
    public int transition;
    public float timeToFade;
    private bool fullFade = false;
    
    void Start()
    {
        current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (current != 0 && images[current - 1].alpha != 0)
        {
            images[current-1].alpha -= timeToFade * Time.deltaTime;
            if (images[current - 1].alpha == 0)
            {
                fullFade = true;
            }
        }
        else if (current == 0 && Mathf.Round(time) == transition)
        {
            current++;
            time = 0;
        }
        
        if (Mathf.Round(time) == transition && current >= 4 && fullFade)
        {
            print("je marche");
            current++;
            SceneManager.LoadScene("good assanblage 1");
        }
        
        if (Mathf.Round(time) == transition && fullFade)
        {
            current++;
            fullFade = false;
            time = 0;
        }
        
    }

    public void skip()
    {
        SceneManager.LoadScene("good assanblage 1");
    }
}
