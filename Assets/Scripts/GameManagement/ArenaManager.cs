using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] GameObject snailPrefab;
    void Start()
    {
        for (var i = 0; i < 10; i++)
        {
            Instantiate(snailPrefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
        }
    }


    
    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(3f, 7f), Random.Range(3f, 7f), 0);
    }
}
