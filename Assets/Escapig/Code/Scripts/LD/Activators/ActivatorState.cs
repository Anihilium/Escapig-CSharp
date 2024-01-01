using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorState : MonoBehaviour
{
    bool activated = false;

    [Tooltip("Should the player stay in order for the activator to be active")]
    public bool MustStay = false;

    [Tooltip("Can activate or deativate the linked object by reusing this activator")]
    public bool Toggleable = false;

    [HideInInspector]
    public bool Activable = false;

    [HideInInspector]
    public List<ActivableState> ActivableStates = new List<ActivableState>();

    public bool GetActive()
    {
        return activated;
    }

    public void SetActive(bool active)
    {
        activated = active;

        if (ActivableStates.Count <= 0)
            return;

        foreach (ActivableState activable in ActivableStates)
            activable.ActivatorChange();
    }

    public void ToggleActive()
    {
        activated = !activated;
        foreach (ActivableState activable in ActivableStates)
            activable.ActivatorChange();
    }

    void OnTriggerEnter(Collider other)
    {
        Activable = true;
    }

    void OnTriggerExit(Collider other)
    {
        Activable = false;
    }
}
