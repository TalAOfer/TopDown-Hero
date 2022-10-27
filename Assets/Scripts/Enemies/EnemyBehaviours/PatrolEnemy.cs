using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PatrolEnemy : MonoBehaviour
{
    private Vector3 startingPosition;
    private Vector3 roamTargetPosition;

    private bool once;
    private Animator anim;
    
    private float distanceFromArrival = 0.1f;

    [SerializeField]
    float
    minDistanceFromStart = 10f,
    maxDistanceFromStart = 70f,
    roamSpeed = 2f,
    minWaitTime = 1.5f,
    maxWaitTime = 3;

    private string prevDirection = "";
    private string newDirection = "down";

    private void Start()
    {
        anim = GetComponent<Animator>();
        startingPosition = transform.position;
        roamTargetPosition = GetNextRoamingPosition();
        HandleWalkingAnimation();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, roamTargetPosition, roamSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, roamTargetPosition) < distanceFromArrival
            && !once)
        {
            once = true;
            HandleIdleAnimation();
            StartCoroutine(Wait());
        }    
    }

    private Vector3 GetNextRoamingPosition()
    {
        return startingPosition + GetRandomDir() * Random.Range(minDistanceFromStart, maxDistanceFromStart);
    }

    private Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1, 1f), Random.Range(-1, 1f)).normalized;
    }

    private float GetRandomTime()
    {
        return Random.Range(minWaitTime, maxWaitTime);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(GetRandomTime());
        roamTargetPosition = GetNextRoamingPosition();
        HandleWalkingAnimation();
        once = false;
    }

    private void HandleWalkingAnimation ()
    {
        float xDistance = roamTargetPosition.x - transform.position.x;
        float yDistance = roamTargetPosition.y - transform.position.y;
        float xAbsDis = Mathf.Abs(xDistance);
        float yAbsDis = Mathf.Abs(yDistance);

        if (xDistance > 0 && xAbsDis >= yAbsDis)
        {
            anim.Play("Walk_Right");
            prevDirection = newDirection;
            newDirection = "right";
        }
        else if (xDistance < 0 && xAbsDis >= yAbsDis)
        {
            anim.Play("Walk_Left");
            prevDirection = newDirection;
            newDirection = "left";
        }
        else if (yDistance > 0 && yAbsDis > xAbsDis)
        {
            anim.Play("Walk_Up");
            prevDirection = newDirection;
            newDirection = "up";
        }
        else if (yDistance < 0 && yAbsDis > xAbsDis)
        {
            anim.Play("Walk_Down");
            prevDirection = newDirection;
            newDirection = "down";
        }

    }

    private void HandleIdleAnimation()
    {
        switch(newDirection)
        {
            case "left":
                anim.Play("Idle_Left");
                break;
            case "right":
                anim.Play("Idle_Right");
                break;
            case "up":
                anim.Play("Idle_Up");
                break;
            case "down":
                anim.Play("Idle_Down");
                break;
        }
    }
}
