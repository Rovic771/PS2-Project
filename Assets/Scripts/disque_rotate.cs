using UnityEngine;

// gaffe aux noms de classes qui suivent pas les conventions du projet ( faut crier Aksel, mais pas trop fort c'est déja bien qu'il dev )
public class disque_rotate : MonoBehaviour
{
    RectTransform rectTransform;
    float angle = 0f;
    [SerializeField] float vitesse;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Update is called once per frame
    void Update()
    {
        angle -= vitesse*Time.deltaTime;
        rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
