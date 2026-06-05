using UnityEngine;

public class disque_rotate : MonoBehaviour
{
    RectTransform rectTransform;
    float angle = 0f;
    [SerializeField] float vitesse = 50f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        print("je fonctionne");
        angle -= vitesse*Time.deltaTime;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
