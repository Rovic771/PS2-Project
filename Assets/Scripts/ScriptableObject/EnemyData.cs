using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    // attention tes variables ne sont pas du tout protégées, si quelqu'un chope
    // ton scriptable et fait life-= truc, c'est finito pour toujours
    public int life;
    public int damage;
    public float speed;
}
