using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class PlayerManager : MonoBehaviour
{

    private PlayerState currentPig;
    private Camera mainCamera;

    [Header("Pigs")]
    [SerializeField] private PlayerState normalPig;
    [SerializeField] private PlayerState strongPig;
    [SerializeField] private PlayerState gaseousPig;
    [SerializeField] private PlayerState miniPig;

    private FMOD.Studio.EventInstance instanceSwitchSound;
    public FMODUnity.EventReference switchSound;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        mainCamera.transform.LookAt(normalPig.transform);
        currentPig = normalPig;
        mainCamera.GetComponent<CameraMovement>().SetPlayerTransform(currentPig.transform);
        currentPig.ActivatePlayer();
        currentPig.FreePig();
        instanceSwitchSound = FMODUnity.RuntimeManager.CreateInstance(switchSound);

    }

    public void SwitchPlayer(InputAction.CallbackContext context)
    {
        if (PauseMenu.bOnPause)
            return;

        if (context.started && !mainCamera.GetComponent<CameraMovement>().IsOnSwitching() && !mainCamera.GetComponent<CameraMovement>().IsOnPipe() && currentPig.GetComponent<Movement>().IsGrounded() && !currentPig.HasBeenThrow())
        {
            Vector2 DPad = context.ReadValue<Vector2>();

            if (DPad.x > 0 && strongPig.IsFree() && !strongPig.IsCurrentPlayer())
            {
                instanceSwitchSound.setParameterByNameWithLabel("character", "ElPiggo");
                instanceSwitchSound.start();
                DoSwitch(strongPig);
            }
            else if(DPad.x < 0 && miniPig.IsFree() && !miniPig.IsCurrentPlayer())
            {
                instanceSwitchSound.setParameterByNameWithLabel("character", "Justine");
                instanceSwitchSound.start();
                DoSwitch(miniPig);
            }
            else if(DPad.y > 0 && normalPig.IsFree() && !normalPig.IsCurrentPlayer())
            {
                instanceSwitchSound.setParameterByNameWithLabel("character", "Dandidou");
                instanceSwitchSound.start();
                DoSwitch(normalPig);
            }
            else if(DPad.y < 0 && gaseousPig.IsFree() && !gaseousPig.IsCurrentPlayer())
            {
                instanceSwitchSound.setParameterByNameWithLabel("character", "PetRickh");
                instanceSwitchSound.start();
                DoSwitch(gaseousPig);
            }
        }
    }

    private void DoSwitch(PlayerState newPig)
    {
        if(newPig.IsFree() && newPig != currentPig)
        {
            currentPig.DesactivatePlayer();
            currentPig = newPig;
            currentPig.ActivatePlayer();
            mainCamera.GetComponent<CameraMovement>().FocusNewPlayer(currentPig.transform);
        }
    }

    public void StopCurrentPlayer()
    {
        currentPig.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void FreePig(PigType pigType)
    {
        switch (pigType)
        {
            case PigType.STRONG:
            {
                strongPig.FreePig();
                break;
            }
            case PigType.SMALL:
            {
                miniPig.FreePig();
                break;
            }
            case PigType.GAS:
            {
                gaseousPig.FreePig();
                break;
            }
            default:
            {
                break;
            }
        }
    }
    public PlayerState GetCurPig() { return currentPig; }
}
