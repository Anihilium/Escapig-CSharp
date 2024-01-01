using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInTriggerBox : MonoBehaviour
{
    [SerializeField] private Canvas UiPopUp;
    [SerializeField] private List<PigType> typesCanActivate;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        UiPopUp.transform.GetChild(0).transform.rotation = cam.transform.rotation;
    }

    public void Enable()
    {
        UiPopUp.enabled = true;
    }

    public void Disable()
    {
        if (UiPopUp.enabled)
            UiPopUp.enabled = false;
    }

    public bool CanEnable(PigType playerType)
    {

        foreach (PigType type in typesCanActivate)
        {
            if (playerType == type)
                return true;
        }

        return false;
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerState playerState = other.transform.GetComponent<PlayerState>();

        if (playerState.IsCurrentPlayer())
        {
            if (other.transform.GetComponent<CheckPopUp>() != null && other.transform.GetComponent<CheckPopUp>().CanHavePopUp())
            {
                if (CanEnable(playerState.GetPigType()))
                    UiPopUp.enabled = true;
            }
            else
            {
                Disable();
            }
        }
        else
        {
            Disable();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<CheckPopUp>() != null)
        {
            Disable();
        }
    }
}
