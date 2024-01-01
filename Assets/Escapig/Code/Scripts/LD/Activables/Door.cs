using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : Activables
{
    [Tooltip("Distance the door will travel when openened (put a negative value if you want it to go the other way)")]
    public float SlidingDistance = 2f;

    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        Toggleable = false;
        startingPosition = transform.position;
    }

    public override void Activate(bool p_on)
    {
        if (Toggleable || alreadyActivated)
            return;

        alreadyActivated = true;

        curTime = Time.deltaTime;
    }

    void Update()
    {
        if (curTime <= 0f || animationEnded)
            return;

        transform.position = Vector3.Lerp(startingPosition, startingPosition + transform.right * SlidingDistance, curTime / LerpTime);

        if(curTime >= LerpTime)
        {
            animationEnded = true;
            transform.position = startingPosition + transform.right * SlidingDistance;
        }

        curTime += Time.deltaTime;
    }
}
