using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    ActivatorState state;
    PlayerManager playerManager;
    Transform pressTransform;

    void Start()
    {
        state = GetComponent<ActivatorState>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        pressTransform = transform.Find("press");
    }

    public void OnInteraction(InputAction.CallbackContext callbackContext)
    {
        if (!state || !callbackContext.performed || !state.Activable || (!state.Toggleable && state.GetActive()))
            return;

        if(state.Toggleable)
            state.ToggleActive();
        else
            state.SetActive(true);

        if(state.GetActive())
            pressTransform.localPosition = new Vector3(0f, 0.45f, 0f);
        else
            pressTransform.localPosition = new Vector3(0f, 0.7f, 0f);
    }

    void OnTriggerStay(Collider other)
    {
        state.Activable = playerManager.GetCurPig().gameObject == other.gameObject;
    }
}
