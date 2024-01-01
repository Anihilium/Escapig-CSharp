using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class EnterPipe : MonoBehaviour
{

    [SerializeField] float distanceEnterPipe;
    private CameraMovement cameraMovement;
    private PlayerState playerState;
    private PlayerRaycast playerRaycast;
    [SerializeField]  private FMODUnity.StudioEventEmitter emmiterPipe;

    ParticleSystem particles;

    // Start is called before the first frame update
    void Start()
    {
        cameraMovement = Camera.main.GetComponent<CameraMovement>();
        playerState = GetComponent<PlayerState>();
        playerRaycast = GetComponent<PlayerRaycast>();
        particles = transform.Find("Smoke_1").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Enter(InputAction.CallbackContext value)
    {
        if (PauseMenu.bOnPause)
            return;

        if (value.started && !cameraMovement.IsOnPipe() && playerState.IsCurrentPlayer())
        {
            GameObject hitObject = playerRaycast.GetHitObject();
            if (hitObject != null && hitObject.transform.parent != null && hitObject.transform.parent.transform.parent != null)
            {
                if (hitObject.transform.parent.transform.parent.GetComponent<Pipe>() != null)
                {
                    hitObject.transform.parent.transform.parent.GetComponent<Pipe>().EnterPipe(hitObject.transform.position);
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    transform.GetChild(0).gameObject.SetActive(false);
                    GetComponent<CheckPopUp>().SetCanHavePopUp(false);
                    particles.transform.Rotate(new Vector3(0f, 180f, 0f));
                    particles.Play();
                    emmiterPipe.Play();
                }
            }
        }
    }

    public void ExitPipe(Vector3 endPosition, Vector3 finalDirection)
    {
        transform.position = endPosition;
        if (finalDirection.y > 0.8f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        else
        {
            finalDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(finalDirection);
        }

        transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<CheckPopUp>().SetCanHavePopUp(true);
        emmiterPipe.Stop();
        particles.transform.Rotate(new Vector3(0f, 180f, 0f));
        particles.Play();
    }
}
