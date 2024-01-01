using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GrabBox : MonoBehaviour
{
    [SerializeField] private float multiplierSpeed = 0.5f;
    [SerializeField] private float distanceToGrab;
    [SerializeField] private float distanceFromBoxWhenGrabbing;
    [SerializeField] private BoxCollider colliderWithoutBox;
    [SerializeField] private BoxCollider colliderWithBox;


    private bool bIsGrabing;
    private Transform box;
    private Vector3 directionGrab;
    private Vector3 directionGrabInViewport;
    private PlayerState playerState;
    private PlayerRaycast playerRaycast;
    private Animator animator;
    private float distancePlayerBox;


    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerRaycast = GetComponent<PlayerRaycast>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bIsGrabing)
        {
            Vector3 centerViewport = Camera.main.WorldToViewportPoint(transform.position);
            Vector3 directionViewport = Camera.main.WorldToViewportPoint(transform.position + 5 * directionGrab.normalized) - centerViewport;
            directionGrabInViewport = directionViewport.normalized;

            if (distancePlayerBox < Vector3.Distance(transform.position, box.transform.position) - 0.05)
            {
                UnGrabBox();
            }
        }
    }
    
    public void Grab(InputAction.CallbackContext value)
    {

        if (!PauseMenu.bOnPause && value.started && playerState.IsCurrentPlayer())
        {
            GameObject hitObject = playerRaycast.GetHitObject();
            if (hitObject != null && hitObject.transform.GetComponent<Box>() != null)
            {
                DoGrabBox(hitObject);
            }
        }

        if(value.canceled && bIsGrabing && playerState.IsCurrentPlayer())
        {
            UnGrabBox();
        }
    }

    public void DoGrabBox(GameObject hitObject)
    {
        bIsGrabing = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        box = hitObject.transform;
        directionGrab = FindDirectionGrab(transform.forward);
        Vector3 newPlayerPos = box.position - (directionGrab * box.localScale.x) - (directionGrab * distanceFromBoxWhenGrabbing);
        transform.position = new Vector3(newPlayerPos.x, transform.position.y, newPlayerPos.z);
        transform.rotation = Quaternion.LookRotation(directionGrab);
        box.SetParent(transform, true);
        colliderWithoutBox.enabled = false;
        colliderWithBox.enabled = true;
        box.gameObject.layer = 10;

        Vector3 centerViewport = Camera.main.WorldToViewportPoint(box.transform.position);
        Vector3 directionViewport = Camera.main.WorldToViewportPoint(box.transform.position + 5 * directionGrab.normalized) - centerViewport;
        directionGrabInViewport = directionViewport.normalized;
        GetComponent<CheckPopUp>().SetCanHavePopUp(false);
        animator.SetTrigger("push");
        distancePlayerBox = Vector3.Distance(transform.position, box.transform.position);
    }

    public void UnGrabBox()
    {
        bIsGrabing = false;
        box.SetParent(null);
        GetComponent<CheckPopUp>().SetCanHavePopUp(true);
        animator.SetTrigger("endPush");
        box.gameObject.layer = 7;
        colliderWithoutBox.enabled = true;
        colliderWithBox.enabled = false;
    }

    Vector3 FindDirectionGrab(Vector3 forwardPlayer)
    {
        if(Mathf.Abs(forwardPlayer.x) > Mathf.Abs(forwardPlayer.z))
        {
            if (forwardPlayer.x > 0)
                return Vector3.right;
            else
                return Vector3.left;
        }
        else
        {
            if (forwardPlayer.z > 0)
                return Vector3.forward;
            else
                return Vector3.back;
        }
    }

    public bool IsGrabbing() { return bIsGrabing; }
    public Vector3 GetGrabDirection() { return directionGrab; }
    public Vector3 GetGrabDirectionViewport() { return directionGrabInViewport; }
    public float GetMultiplierSpeed() { return multiplierSpeed; }

}

