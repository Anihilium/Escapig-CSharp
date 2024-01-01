using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    PlayerManager managerScript;
    PlayerState lastPig;

    public List<Image> playerImages = new List<Image>();

    // Start is called before the first frame update
    void Start()
    {
        managerScript = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        lastPig = managerScript.GetCurPig();
        playerImages.Add(transform.Find("DandidouUI").GetComponent<Image>());
        playerImages.Add(transform.Find("ElPiggoUI").GetComponent<Image>());
        playerImages.Add(transform.Find("JustineUI").GetComponent<Image>());
        playerImages.Add(transform.Find("PetTrickUI").GetComponent<Image>());
        for (int i = 0; i < managerScript.transform.childCount - 1; i++)
            managerScript.transform.GetChild(i).GetComponent<PlayerState>().selectorUI = this;


    }

    // Update is called once per frame
    void Update()
    {
        if (managerScript.GetCurPig() == lastPig)
            return;

        lastPig = managerScript.GetCurPig();

        foreach (Image img in playerImages)
            img.color = new Color(img.color.r, img.color.g, img.color.b, .2f);

        Image curImage = playerImages[(int)managerScript.GetCurPig().GetPigType()];

        curImage.color = new Color(curImage.color.r, curImage.color.g, curImage.color.b, 1f);
    }

    public void Show(PlayerState p_pig)
    {
        playerImages[(int)p_pig.GetPigType()].enabled = true;
    }
}
