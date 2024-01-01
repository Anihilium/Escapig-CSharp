using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Activables
{
    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 destination;
    float distance = 1f;
    float maxDistance = 1f;
    Transform elevatorPlate;

    private FMOD.Studio.EventInstance instance;

    public FMODUnity.EventReference fmodEvent;




    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        endPosition = transform.Find("EndPosition").position;
        elevatorPlate = transform.Find("ElevatorPlate");
        maxDistance = Vector3.Distance(startPosition, endPosition);

        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
    }

    public override void Activate(bool p_on)
    {
        if (!Toggleable && alreadyActivated)
            return;

        animationEnded = false;

        instance.setParameterByNameWithLabel("elevator", "on");
        instance.start();

        if (p_on)
            destination = endPosition;
        else
            destination = startPosition;

        distance = Vector3.Distance(elevatorPlate.position, destination);

        curTime = LerpTime * (1f - (distance / maxDistance)) + Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (curTime <= 0f || animationEnded)
            return;

        Vector3 start = destination == startPosition ? endPosition : startPosition;
        
        elevatorPlate.position = Vector3.Lerp(start, destination, curTime / LerpTime);

        if(curTime >= LerpTime)
        {
            elevatorPlate.position = destination;
            animationEnded  = true;
            curTime = 0f;

            instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instance.setParameterByNameWithLabel("elevator", "off");
        }

        curTime += Time.deltaTime;
    }
}
