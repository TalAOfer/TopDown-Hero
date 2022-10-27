using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 target;
    public float slideSpeed = 2f;
    private bool move = false;
    private bool once = false;
    void Update()
    {
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, slideSpeed * Time.deltaTime);
        }

        if (transform.position == target)
        {
            move = false;
            once = false;
        }
    }

    public void EnableMove(Component sender, object data)
    {
        if (once) return;
        string direction = (string) data;
        Vector2 directionVector = new Vector2 ();

            Debug.Log("direction:" + direction);
            switch (direction)
            {
                case ("r"):
                    directionVector.x = 18;
                    break;
                case ("l"):
                    directionVector.x = -18;
                    break;
                case ("u"):
                    directionVector.y = 10;
                    break;
                case ("d"):
                    directionVector.y = -10;
                    break;
            }

            target = new Vector3(transform.position.x + directionVector.x, transform.position.y + directionVector.y, transform.position.z);
            move = true;
            once = true;
    }
}
