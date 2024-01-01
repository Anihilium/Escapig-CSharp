using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FartUI : MonoBehaviour
{
    public Sprite EmptyFartImage;
    public Sprite FullFartImage;
    List<Image> fartImageList = new List<Image>();

    PlayerManager playerManager;
    PlayerState lastPig;

    GameObject fartParent;

    Fart fartScript;
    int lastFartAmount = 3;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        lastPig = playerManager.GetCurPig();
        fartParent = transform.Find("FartParent").gameObject;
        foreach(Transform child in fartParent.transform)
            fartImageList.Add(child.GetComponent<Image>());
        fartScript = playerManager.transform.GetChild(2).GetComponent<Fart>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerState curPig = playerManager.GetCurPig();

        if(lastPig != curPig)
        {
            if(curPig.GetPigType() == PigType.GAS)
                fartParent.SetActive(true);
            else
                fartParent.SetActive(false);
        }

        lastPig = curPig;

        if (curPig.GetPigType() != PigType.GAS)
            return;

        if(fartScript.fartsAvailable != lastFartAmount)
        {
            for(int i = 0; i < fartScript.fartsAvailable; i++)
                fartImageList[i].sprite = FullFartImage;

            for (int i = 0; i < fartScript.maxFarts - fartScript.fartsAvailable; i++)
                fartImageList[fartScript.maxFarts - i - 1].sprite = EmptyFartImage;

            lastFartAmount = fartScript.fartsAvailable;
        }
    }
}
