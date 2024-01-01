using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    ActivatorState state;
    Transform pressTransform;

    void Start()
    {
        state = GetComponent<ActivatorState>();
        pressTransform = transform.Find("press");
    }

    void OnTriggerEnter()
    {
        if (!state)
            return;

        if (state.Toggleable)
            state.ToggleActive();
        else
            state.SetActive(true);

        if (state.GetActive())
            pressTransform.localPosition = new Vector3(0f, 0.05f, 0f);
        else
            pressTransform.localPosition = new Vector3(0f, 0.2f, 0f);
    }

    void OnTriggerExit()
    {
        if (!state)
            return;

        if (state.MustStay)
            state.SetActive(false);

        if (state.GetActive())
            pressTransform.localPosition = new Vector3(0f, 0.05f, 0f);
        else
            pressTransform.localPosition = new Vector3(0f, 0.2f, 0f);
    }
}
