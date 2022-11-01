using UnityEngine;

[CreateAssetMenu(fileName="Data", menuName = "ScriptableObjects/PlayerForm")]
public class PlayerForm_SO : ScriptableObject
{
    public int damage;

    public float speed;

    public bool isStuckWhenAttacking;

    public Vector3 attackPointRight,
                    attackPointLeft,
                    attackPointUp,
                    attackPointDown;
}
