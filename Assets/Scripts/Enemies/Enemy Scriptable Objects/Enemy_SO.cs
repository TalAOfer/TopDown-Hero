using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Data", menuName = "ScriptableObjects/Enemy")]
public class Enemy_SO : ScriptableObject
{
    public int hp,
               damage;

    public float speed;

}
