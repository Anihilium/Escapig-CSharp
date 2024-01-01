using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class EatCarrot : MonoBehaviour
{
    private PlayerState playerState;
    private PlayerRaycast playerRaycast;
    private Fart fart;

    ParticleSystem particles;


    [SerializeField] FMODUnity.StudioEventEmitter emitterEatCarrot;

    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<PlayerState>();
        playerRaycast = GetComponent<PlayerRaycast>();
        fart = GetComponent<Fart>();
        particles = transform.Find("pCarrot").GetComponent<ParticleSystem>();
    }

    public void DoEat(InputAction.CallbackContext value)
    {
        if (PauseMenu.bOnPause)
            return;

        if (!PauseMenu.bOnPause && value.started && playerState.IsCurrentPlayer())
        {
            GameObject hitObject = playerRaycast.GetHitObject();
            if (hitObject != null && hitObject.GetComponent<Carrot>() && fart.CanEat())
            {
                emitterEatCarrot.Play();
                fart.Reload(); 
                Destroy(hitObject);
                particles.Play();
            }
        }
    }
}
