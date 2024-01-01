using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public enum PigType : int
{ 
    NORMAL,
    STRONG,
    SMALL,
    GAS,
}

public class PlayerState : MonoBehaviour
{
    // Start is called before the first frame update
    private bool bCurrentPlayer;
    private bool bFree;
    private bool bHasBeenThrow;
    [SerializeField] PigType type;
    [SerializeField] GameObject UIImageFree;
    [SerializeField] GameObject UIBarresNoires;

    [SerializeField] FMODUnity.StudioEventEmitter emitterLand;

    private Rigidbody rb;
    private Movement movement;
    private CameraMovement cameraMovement;
    private Animator animator;

    [HideInInspector]
    public PlayerSelector selectorUI;
    public GameObject uiCanva;


    void Start()
    {
        bFree = false; //DOTO : Only for testing change to false later
        bCurrentPlayer = false;
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints -= RigidbodyConstraints.FreezePositionY;

        animator = GetComponent<Animator>();
        cameraMovement = Camera.main.transform.GetComponent<CameraMovement>();
    }

    public void ActivatePlayer()
    {
        bCurrentPlayer = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        GetComponent<FMODUnity.StudioListener>().enabled = true;
        animator.enabled = true;
    }

    public void DesactivatePlayer()
    {
        bCurrentPlayer = false;
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.constraints -= RigidbodyConstraints.FreezePositionY;
        GetComponent<FMODUnity.StudioListener>().enabled = false;
        animator.enabled = false;

    }

    public void FreePig()
    {
        cameraMovement.FreePlayerCameraAnim(this, UIImageFree);
        selectorUI.Show(this);

        if(uiCanva)
            uiCanva.SetActive(false);

        UIBarresNoires.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(bHasBeenThrow && collision.transform.GetComponent<PlayerState>() == null)
        {
            bHasBeenThrow = false;
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.constraints -= RigidbodyConstraints.FreezePositionY;

            emitterLand.Play();
        }
    }

    public bool IsCurrentPlayer() { return bCurrentPlayer; }
    public bool IsFree() { return bFree; }
    public void SetFree(bool free) 
    { 
        bFree = free;

        if(free && uiCanva)
            uiCanva.SetActive(true);

        UIBarresNoires.SetActive(false);

    }


    public bool HasBeenThrow() { return bHasBeenThrow; }

    public void SetHasBeenThrow(bool hasThrow) { bHasBeenThrow = hasThrow; }
    public PigType GetPigType() { return type; }


}
