using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableState : MonoBehaviour
{
    [Tooltip("Add buttons and pressure plates to this list")]
    public List<ActivatorState> activators = new List<ActivatorState>();

    [Tooltip("Link the door / pen / elevator script here")]
    public Activables activable;

    bool lastState = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach (ActivatorState activator in activators)
            activator.ActivableStates.Add(this);
    }

    public void ActivatorChange()
    {
        bool allActivatorsOn = true;

        foreach (ActivatorState activator in activators)
        {
            if (!activator.GetActive())
            {
                allActivatorsOn = false;
                break;
            }
        }

        if((activable.Toggleable && lastState != allActivatorsOn) || allActivatorsOn)
            activable.Activate(allActivatorsOn);

        lastState = allActivatorsOn;
    }
}
