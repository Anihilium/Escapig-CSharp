using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    private bool bCanHavePopUp;
    private PlayerRaycast playerRaycast;
    private PlayerState playerState;
    private PopUp currentPopUp;

    void Start()
    {
        playerRaycast = GetComponent<PlayerRaycast>();
        playerState = GetComponent<PlayerState>();
        bCanHavePopUp = true;
        currentPopUp = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerState.IsCurrentPlayer() && bCanHavePopUp)
        {
            GameObject hitObject = playerRaycast.GetHitObject();
            if (hitObject == null)
            {
                DisableCurrentPopUp();
                return;
            }

            PopUp popUp = hitObject.GetComponent<PopUp>();

            if (popUp != null && popUp.CanEnable(playerState.GetPigType()))
            {
                if (popUp == currentPopUp)
                    return;

                popUp.Enable(playerState.GetPigType());
                if(currentPopUp != null)
                    currentPopUp.Disable();
                currentPopUp = popUp;
            }
            else
            {
                if(currentPopUp != null)
                {
                    currentPopUp.Disable();
                    currentPopUp = null;
                }
            }

        }
        else
        {
            if(currentPopUp != null)
            {
                currentPopUp.Disable();
                currentPopUp = null;
            }
        }
    }

    public void DisableCurrentPopUp()
    {
        if(currentPopUp != null)
        {
            currentPopUp.Disable();
            currentPopUp = null;
        }
    }

    public void SetCanHavePopUp(bool can) { bCanHavePopUp = can; }
    public bool CanHavePopUp() { return bCanHavePopUp; }

}
