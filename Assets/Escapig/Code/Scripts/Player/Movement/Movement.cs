using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    private Vector2 movementRaw;
    private Rigidbody rb;
    private Transform cam;
    private CameraMovement camMovement;

    private PlayerState playerState;
    private bool bIsGrounded;
    private bool bIsJumping;
    private bool bJumpCanceled;
    private float cdJump;

    private GrabBox grabBox;
    private GrabAndThrow GrabAndThrow;
    private Fart fart;

    [SerializeField] private float Speed = 5.0f;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float gravityJumpMult;
    [SerializeField] private float gravityFallMult;
    [SerializeField] private float distanceRayGrounded;
    [SerializeField] FMODUnity.StudioEventEmitter emitterMoveBox;

    private FMOD.Studio.EventInstance instanceJumpSound;
    public FMODUnity.EventReference jumpSound;

    private Animator animator;


    public void Start()
    {
        cam = Camera.main.transform;
        camMovement = cam.GetComponent<CameraMovement>();
        grabBox = GetComponent<GrabBox>();
        GrabAndThrow = GetComponent<GrabAndThrow>();
        fart = GetComponent<Fart>();
        movementRaw = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        playerState = GetComponent<PlayerState>();
        cdJump = 0f;
        animator = GetComponent<Animator>();

        instanceJumpSound = FMODUnity.RuntimeManager.CreateInstance(jumpSound);
    }

    private void Update()
    {
        if (PauseMenu.bOnPause)
            return;

        UpdateGrounded();

        if (cdJump > 0f)
        {
            cdJump -= Time.deltaTime;
        }
    }

    public void FixedUpdate()
    {
        if (PauseMenu.bOnPause)
            return;

        if (playerState.IsCurrentPlayer() && !camMovement.IsOnSwitching() && !camMovement.IsOnPipe())
        {
            
            if (grabBox != null && grabBox.IsGrabbing())
            {
                MoveWithBox();
            }
            else if(GrabAndThrow != null && GrabAndThrow.IsGrabbing())
            {
                MoveWithPig();
            }
            else
            {
                Move();
                if (emitterMoveBox.IsPlaying())
                    emitterMoveBox.Stop();
            }

            // Jump gravity
            if (animator && animator.enabled)
                animator.SetFloat("velocityY", rb.velocity.y);

            if (rb.velocity.y < 0 || bJumpCanceled)
                rb.velocity += gravityFallMult * Physics.gravity.y * Time.deltaTime * Vector3.up;
            else if (rb.velocity.y > 0)
                rb.velocity += gravityJumpMult * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
    }

    public void Move()
    {
        Vector3 fwd = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;
        Vector3 rgt = new Vector3(cam.right.x, 0f, cam.right.z).normalized;
        Vector3 combine = (fwd * movementRaw.y + rgt * movementRaw.x).normalized;

        rb.velocity = combine * Speed + Vector3.up * rb.velocity.y;

        if (combine != Vector3.zero)
        {
            if (animator && animator.enabled)
                animator.SetFloat("speed", 10);
            transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(combine.x, combine.z) * Mathf.Rad2Deg, 0f);
        }
        else
        {
            if (animator && animator.enabled)
                animator.SetFloat("speed", -10);
        }
    }

    public void MoveWithBox()
    {
        if (movementRaw.magnitude > 0.1f)
        {
            if(!emitterMoveBox.IsPlaying())
                emitterMoveBox.Play();

            float angleMovementAndGrabDir = Vector2.Angle(grabBox.GetGrabDirectionViewport(), new Vector3(movementRaw.x, movementRaw.y, 0));
            if (angleMovementAndGrabDir < 45f)
            {
                rb.velocity = grabBox.GetMultiplierSpeed() * Speed * grabBox.GetGrabDirection().normalized;
            }
            else if (angleMovementAndGrabDir > 135f)
            {
                rb.velocity = -grabBox.GetMultiplierSpeed() * Speed * grabBox.GetGrabDirection().normalized;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
            if (emitterMoveBox.IsPlaying())
                emitterMoveBox.Stop();
        }
    }

    public void MoveWithPig()
    {
        Vector3 fwd = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;
        Vector3 rgt = new Vector3(cam.right.x, 0f, cam.right.z).normalized;
        Vector3 combine = (fwd * movementRaw.y + rgt * movementRaw.x).normalized;

        if (combine != Vector3.zero)
            transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(combine.x, combine.z) * Mathf.Rad2Deg, 0f);
    }


    public void OnMovement(InputAction.CallbackContext value)
    {
        movementRaw = value.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext value)
    {
        if (PauseMenu.bOnPause || playerState.GetPigType() == PigType.STRONG)
            return;

        if (playerState.IsCurrentPlayer() && !camMovement.IsOnSwitching() && !camMovement.IsOnPipe())
        {
            if (GetComponent<GrabBox>() == null || !GetComponent<GrabBox>().IsGrabbing()) //Not Currently grabbing
            {
                if (value.started)
                {
                    if (bIsGrounded && !bIsJumping && cdJump <= 0f)
                    {
                        instanceJumpSound.start();
                        if (animator && animator.enabled)
                            animator.SetTrigger("jump");

                        switch (playerState.GetPigType())
                        {
                            case PigType.NORMAL:
                            {
                                instanceJumpSound.setParameterByNameWithLabel("character", "Dandidou");
                                break;
                            }
                            case PigType.SMALL:
                            {
                                instanceJumpSound.setParameterByNameWithLabel("character", "Justine");
                                break;
                            }
                            case PigType.GAS:
                            {
                                instanceJumpSound.setParameterByNameWithLabel("character", "PetRickh");
                                break;
                            }
                            default:
                                break;

                        }

                        rb.velocity = Vector3.up * jumpVelocity;
                        bIsJumping = true;
                        cdJump = 0.25f;
                    }
                    else if(bIsJumping && fart != null)
                    {
                        fart.DoFart();
                        if (animator && animator.enabled)
                            animator.SetTrigger("jump");
                    }
                }

                if (value.canceled)
                {
                    bJumpCanceled = true;
                }
            }
        }

    }

    public void UpdateGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        Debug.DrawRay(transform.position, Vector3.down * distanceRayGrounded);

        if (Physics.Raycast(ray, distanceRayGrounded))
        {
            bIsGrounded = true;
            bJumpCanceled = false;

            if(cdJump <= 0 && bIsJumping)
            {
                bIsJumping = false;
                if (animator && animator.enabled)
                    animator.SetTrigger("endJump");
            }
        }
        else
        {
            bIsGrounded = false;
        }
    }

    public bool IsGrounded() { return bIsGrounded; }
    public bool IsJumping() { return bIsJumping; }
}
