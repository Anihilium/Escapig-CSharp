using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GrabCarrot : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Carrot> nearCarrots;
    private Carrot holdingCarrot;

    private PlayerState playerState;
    private GrabAndThrow grabThrow;
    private GrabBox grabBox;
    private PlayerRaycast playerRaycast;

    [SerializeField] private float distanceHold = 1.5f;

    [SerializeField]  FMODUnity.StudioEventEmitter emitterGrabCarrot;

    void Start()
    {
        playerState = GetComponent<PlayerState>();
        grabThrow = GetComponent<GrabAndThrow>();
        grabBox = GetComponent<GrabBox>();
        playerRaycast = GetComponent<PlayerRaycast>();

        nearCarrots = new List<Carrot>();
    }

    public void AddNearCarrot(Carrot carrot)
    {
        nearCarrots.Add(carrot);
    }

    public void RemoveNearCarrot(Carrot carrot)
    {
        nearCarrots.Remove(carrot);
    }

    public void DoGrabCarrot(InputAction.CallbackContext value)
    {

        if (!PauseMenu.bOnPause && value.started && playerState.IsCurrentPlayer())
        {
            GameObject hitObject = playerRaycast.GetHitObject();
            if (hitObject != null && hitObject.GetComponent<Carrot>())
            {
                holdingCarrot = hitObject.GetComponent<Carrot>();
                holdingCarrot.transform.SetParent(transform);
                holdingCarrot.transform.position = transform.position + transform.forward * GetComponent<CapsuleCollider>().radius * distanceHold;
                holdingCarrot.transform.localRotation = Quaternion.Euler(90, 0, -90);
                holdingCarrot.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                holdingCarrot.GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<CheckPopUp>().SetCanHavePopUp(false);
                emitterGrabCarrot.Play();
            }
        }
        else if (value.canceled && holdingCarrot != null)
        {
            holdingCarrot.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            holdingCarrot.GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionY;
            holdingCarrot.transform.SetParent(null);
            holdingCarrot.GetComponent<CapsuleCollider>().enabled = true;
            holdingCarrot = null;
            GetComponent<CheckPopUp>().SetCanHavePopUp(true);
        }
    }

}
