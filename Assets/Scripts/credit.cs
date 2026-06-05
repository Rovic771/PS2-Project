using UnityEngine;

public class credit : MonoBehaviour
{
    [SerializeField] private float time;
    public float speed;
    private RectTransform rect;

    [SerializeField] private GameObject creditObjet;

    void Start()
    {
        rect = creditObjet.GetComponent<RectTransform>();
    }
    
    void Update()
    {
        time += Time.deltaTime;
        if (time > 5)
        {
            rect.localPosition = new Vector3(rect.localPosition.x, rect.localPosition.y + speed, rect.localPosition.z);
        }
    }
}
