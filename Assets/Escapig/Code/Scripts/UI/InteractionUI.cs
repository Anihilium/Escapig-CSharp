using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerState lastPig;

    GameObject aUI;
    GameObject notAUI;
    GameObject xUI;
    GameObject yUI;
    GameObject rtUI;
    GameObject jumpUI;
    GameObject jumpFartUI;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        lastPig = playerManager.GetCurPig();

        aUI = transform.Find("UI_A").gameObject;
        notAUI = transform.Find("UI_NotA").gameObject;
        xUI = transform.Find("UI_X").gameObject;
        yUI = transform.Find("UI_Y").gameObject;
        rtUI = transform.Find("UI_RT").gameObject;

        jumpUI = aUI.transform.Find("Jump").gameObject;
        jumpFartUI = aUI.transform.Find("JumpFart").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPig == playerManager.GetCurPig())
            return;

        switch (lastPig.GetPigType())
        {
            case PigType.NORMAL:
            case PigType.SMALL:
                DandidouJustineUI(false);
                break;
            case PigType.STRONG:
                ElPiggoUI(false);
                break;
            case PigType.GAS:
                PetTrickhUI(false);
                break;
            default: break;
        }

        lastPig = playerManager.GetCurPig();

        switch (playerManager.GetCurPig().GetPigType())
        {
            case PigType.NORMAL:
            case PigType.SMALL:
                DandidouJustineUI(true);
                break;
            case PigType.STRONG:
                ElPiggoUI(true);
                break;
            case PigType.GAS:
                PetTrickhUI(true);
                break;
            default: break;
        }
    }

    void DandidouJustineUI(bool p_activate)
    {
        aUI.SetActive(p_activate);
        jumpUI.SetActive(p_activate);
        xUI.SetActive(p_activate);
    }

    void ElPiggoUI(bool p_activate)
    {
        notAUI.SetActive(p_activate);
        xUI.SetActive(p_activate);
        rtUI.SetActive(p_activate);
    }

    void PetTrickhUI(bool p_activate)
    {
        aUI.SetActive(p_activate);
        jumpFartUI.SetActive(p_activate);
        xUI.SetActive(p_activate);
        yUI.SetActive(p_activate);
    }
}
