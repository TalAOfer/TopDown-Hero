using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemManager : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    List<Heart> hearts = new List<Heart>();

    private void ClearHearts()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<Heart>();
    }

    private void CreateFullHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        Heart heartComponent = newHeart.GetComponent<Heart>();
        heartComponent.SetHeartImage(HeartStatus.Full);
        hearts.Add(heartComponent);
    }

    public void DrawHearts(Component sender, object data)
    {
        ClearHearts();

        if (data is int[])
        {
            int[] healthData = (int[])data;
            int maxHealth = healthData[0];
            int currentHealth = healthData[1];

            float maxHealthRemainder = maxHealth % 2;
            int heartsToMake = (int)((maxHealth / 2) + maxHealthRemainder);

            for (int i = 0; i < heartsToMake; i++)
            {
                CreateFullHeart();
            }

            for (int i = 0; i < hearts.Count; i++)
            {
                int heartStatusRemainder = (int)Mathf.Clamp(currentHealth - (i * 2), 0, 2);
                hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
            }
        }
    }
}
