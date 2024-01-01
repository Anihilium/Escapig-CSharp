using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GrabAndThrow : MonoBehaviour
{
    [SerializeField] private float distanceToGrab;
    [SerializeField] private float forceThrow;
    [SerializeField] private float angleThrow;
    [SerializeField] private GameObject Arrow;
    private Animator animator;

    private bool bIsGrabbingPig;
    private PlayerState pigGrabbed;
    private PlayerState playerState;
    private PlayerRaycast playerRaycast;

    private FMOD.Studio.EventInstance instanceGrabSound;
    public FMODUnity.EventReference grabSound;

    private FMOD.Studio.EventInstance instanceThrowSound;
    public FMODUnity.EventReference throwSound;

    // Start is called before the first frame update
    void Start()
    {
        bIsGrabbingPig = false;
        pigGrabbed = null;
        playerState = GetComponent<PlayerState>();
        playerRaycast = GetComponent<PlayerRaycast>();
        instanceGrabSound = FMODUnity.RuntimeManager.CreateInstance(grabSound);
        instanceThrowSound = FMODUnity.RuntimeManager.CreateInstance(throwSound);
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Grab(InputAction.CallbackContext value)
    {
        if (PauseMenu.bOnPause)
            return;

        if (value.started &&  playerState.IsCurrentPlayer() && pigGrabbed == null)
        {
            GameObject hitObject = playerRaycast.GetHitObject();
            if (hitObject != null && hitObject.transform.GetComponent<PlayerState>() != null)
            {
                bIsGrabbingPig = true;
                pigGrabbed = hitObject.transform.GetComponent<PlayerState>();
                pigGrabbed.transform.SetParent(transform);
                pigGrabbed.transform.localPosition = new Vector3(0, transform.localScale.y * 2f, 0);
                pigGrabbed.transform.rotation = transform.rotation;

                GetComponent<Rigidbody>().velocity = Vector3.zero;
                Arrow.SetActive(true);
                GetComponent<CheckPopUp>().SetCanHavePopUp(false);
                instanceGrabSound.start();
                animator.SetTrigger("grab");
            }
            
        }
        else if(value.canceled &&  playerState.IsCurrentPlayer() && pigGrabbed != null)
        {
            Arrow.SetActive(false);
            pigGrabbed.transform.SetParent(null);
            pigGrabbed.transform.position = transform.position + transform.forward * transform.localScale.x * 1.5f;
            bIsGrabbingPig = false;
            pigGrabbed = null;
            GetComponent<CheckPopUp>().SetCanHavePopUp(true);
            animator.SetTrigger("endGrab");
        }

    }

    public void Throw(InputAction.CallbackContext value)
    {
        if (PauseMenu.bOnPause)
            return;

        if (value.started && playerState.IsCurrentPlayer() && pigGrabbed != null)
        {
            pigGrabbed.transform.SetParent(null);
            pigGrabbed.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            Vector3 directionThrow = Quaternion.AngleAxis(angleThrow, -pigGrabbed.transform.right) * pigGrabbed.transform.forward;
            pigGrabbed.GetComponent<Rigidbody>().AddForce(directionThrow * forceThrow, ForceMode.Impulse);
            pigGrabbed.SetHasBeenThrow(true);
            bIsGrabbingPig = false;
            pigGrabbed = null;
            Arrow.SetActive(false);
            GetComponent<CheckPopUp>().SetCanHavePopUp(true);
            instanceThrowSound.start();
            animator.SetTrigger("throw");
        }
    }

    public bool IsGrabbing() { return bIsGrabbingPig; }

}
