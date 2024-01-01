using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.ParticleSystem;


public class Fart : MonoBehaviour
{
    [SerializeField] private float jumpForce;
    public int fartsAvailable;
    public int maxFarts;

    ParticleSystem particles;

    private PlayerState playerState;
    private Rigidbody rb;

    private bool HasToFart;

    private FMOD.Studio.EventInstance instance;
    public FMODUnity.EventReference fmodEvent;


    // Start is called before the first frame update
    void Start()
    {
        particles = transform.Find("Smoke_1").GetComponent<ParticleSystem>();

        fartsAvailable = maxFarts;
        HasToFart = false;

        playerState = GetComponent<PlayerState>();
        rb = GetComponent<Rigidbody>();

        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
    }

    public bool CanEat()
    {
        if (fartsAvailable < maxFarts)
            return true;

        return false;
    }

    public void Reload()
    {
        fartsAvailable = maxFarts;
    }

    private void FixedUpdate()
    {
        if (PauseMenu.bOnPause)
            return;

        if (playerState.IsCurrentPlayer() && HasToFart)
        {
            instance.start();
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            HasToFart = false;
            particles.Play();
        }
    }

    public void DoFart()
    {
        if(playerState.IsCurrentPlayer() && fartsAvailable > 0)
        {
            HasToFart = true;
            fartsAvailable -= 1;
        }
    }
}
