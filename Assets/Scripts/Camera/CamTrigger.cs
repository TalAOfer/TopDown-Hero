using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    [SerializeField] string direction = "";

    [Header("OnEnterCamTrigger")]
    public GameEvent OnEnterCamTrigger;

    void Start()
    {
        this.name = direction;
    }

    public void UpdateDirection(Component sender, object data)
    {
        Vector3 playerPosition = (Vector3) data;

        if(direction == "u" || direction == "d")
        {
            //if gate is now below: trigger will move cam down
            if (transform.position.y < playerPosition.y)
            {
                direction = "d";
                //if gate is now above: trigger will move cam up
            }
            else
            {
                direction = "u";
            }
        }

        if (direction == "r" || direction == "l")
        {
            //if gate is now to the left: trigger will move cam left
            if (transform.position.x < playerPosition.x)
            {
                direction = "l";
            }
            //if gate is now to the right: trigger will move cam right
            else
            {
                direction = "r";
            }
        }

        this.name = direction;
    }
}
