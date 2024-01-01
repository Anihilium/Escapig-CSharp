using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private Transform playerTransform;

    //Input System
    private PlayerInput PlayerInput;
    private InputAction _move;

    //Movement
    [Header("Movement")]
    [SerializeField] private float lookXSpeed;
    [SerializeField] private float lookYSpeed;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private float lockAngleUp;
    [SerializeField] private float lockAngleDown;
    private Vector2 move;

    [Header("Switch")]
    //Switching Player
    private bool bOnSwitching;
    private float lerpSwitching;
    [SerializeField] private float SpeedSwitchingFocus;

    [Header("Pipe")]
    //Go Through Pipe
    private bool bOnPipe;
    private float lerpPipe;




    // Start is called before the first frame update
    void Start()
    {
        PlayerInput = GetComponent<PlayerInput>();
        _move = PlayerInput.actions.FindAction("Look");
        _move.performed += ctx => move = ctx.ReadValue<Vector2>();
        _move.canceled += _ => move = Vector2.zero;

        lerpSwitching = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.bOnPause)
            return;

        if(!bOnSwitching && !bOnPipe)
        {
            MoveCamera();
            RotateCamera();
        }
    }

    private void MoveCamera()
    {
        Vector3 playerToCamera = transform.forward;
        transform.position = playerTransform.transform.position - (playerToCamera * distanceFromPlayer);
    }

    private void RotateCamera()
    {
        if (move.x != 0)
        {
            float angleY = move.x * lookYSpeed * Time.deltaTime;
            transform.RotateAround(playerTransform.position, Vector3.up, angleY);
        }

        if (move.y != 0)
        {
            float angleX = -move.y * lookYSpeed * Time.deltaTime;

            Vector3 directionCam = transform.forward;
            Vector3 dirCamWithoutY = new Vector3(directionCam.x, 0, directionCam.z);

            float angleXCam = Vector3.Angle(dirCamWithoutY, directionCam);
            if (directionCam.y > 0)
                angleXCam *= -1;

            bool insideLockRegion = angleXCam <= lockAngleUp && angleXCam >= lockAngleDown;

            if (insideLockRegion || (angleXCam > lockAngleUp && angleX < 0) || (angleXCam < lockAngleDown && angleX > 0))
            {
                transform.RotateAround(playerTransform.position, transform.right, angleX);
            }
        }
    }

    public void FocusNewPlayer(Transform newPlayerTransform)
    {
        if (bOnSwitching)
            StopCoroutine(LerpNewPlayer());

        bOnSwitching = true;
        lerpSwitching = 0;
        playerTransform = newPlayerTransform;

        StartCoroutine(LerpNewPlayer());
    }

    public void FreePlayerCameraAnim(PlayerState playerState, GameObject imageUI)
    {
        if(imageUI)
            imageUI.SetActive(true);
        if (bOnSwitching)
            StopCoroutine(LerpNewPlayer());

        bOnSwitching = true;
        lerpSwitching = 0;

        StartCoroutine(StartAnimFree(playerState, imageUI));
    }

    public void GoThroughPipe(List<GameObject> pipePoints, float speed)
    {
        bOnPipe = true;
        StartCoroutine(CamInPipe(pipePoints, speed));
    }

    IEnumerator LerpNewPlayer()
    {
        Vector3 StartPosition = transform.position;
        Vector3 EndPosition = playerTransform.transform.position - (transform.forward * distanceFromPlayer);

        while (lerpSwitching < 1)
        {
            transform.position = Vector3.Lerp(StartPosition, EndPosition, lerpSwitching);
            lerpSwitching += SpeedSwitchingFocus * Time.deltaTime;
            yield return null;
        }

        transform.position = EndPosition;
        bOnSwitching = false;
    }

    IEnumerator StartAnimFree(PlayerState playerState, GameObject imageUI)
    {
        Vector3 StartPosition = transform.position;
        Vector3 EndPosition = playerState.transform.position - (transform.forward * distanceFromPlayer);

        while (lerpSwitching < 1)
        {
            transform.position = Vector3.Lerp(StartPosition, EndPosition, lerpSwitching);
            lerpSwitching += SpeedSwitchingFocus * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2.0f);

        lerpSwitching = 0f;
        StartPosition = transform.position;
        EndPosition = playerTransform.transform.position - (transform.forward * distanceFromPlayer);

        while (lerpSwitching < 1)
        {
            transform.position = Vector3.Lerp(StartPosition, EndPosition, lerpSwitching);
            lerpSwitching += SpeedSwitchingFocus * Time.deltaTime;
            yield return null;
        }

        transform.position = EndPosition;
        bOnSwitching = false;
        if (imageUI)
            imageUI.SetActive(false);
        playerState.SetFree(true);
    }

    IEnumerator CamInPipe(List<GameObject> pipePoints, float speed)
    {
        int curPipePointIndex = 0;
        int nextPipePointIndex = 1;
        int MaxPoints = pipePoints.Count - 1;
        Vector3 playerToCamera = transform.forward;
        Vector3 StartPosition = Vector3.zero;
        Vector3 EndPosition = Vector3.zero;

        while (nextPipePointIndex <= MaxPoints)
        {
            lerpPipe = 0f;

            StartPosition = pipePoints[curPipePointIndex].transform.position;
            EndPosition = pipePoints[nextPipePointIndex].transform.position;
            float distance = Vector3.Distance(StartPosition, EndPosition);

            while (lerpPipe < 1)
            {
                transform.position = Vector3.Lerp(StartPosition, EndPosition, lerpPipe) - (playerToCamera * distanceFromPlayer);
                lerpPipe += (speed / distance) * Time.deltaTime;
                yield return null;
            }

            transform.position = EndPosition - (playerToCamera * distanceFromPlayer);

            curPipePointIndex++;
            nextPipePointIndex++;
        }

        bOnPipe = false;
        pipePoints.Reverse();
        playerTransform.GetComponent<EnterPipe>().ExitPipe(EndPosition, (EndPosition - StartPosition).normalized);
    }

    public bool IsOnSwitching() { return bOnSwitching; }
    public bool IsOnPipe() { return bOnPipe; }

    public void SetPlayerTransform(Transform transform)
    {
        playerTransform = transform;
    }


}
