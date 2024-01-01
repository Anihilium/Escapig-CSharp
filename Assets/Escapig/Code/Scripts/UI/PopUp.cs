using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PopUp : MonoBehaviour
{
    [System.Serializable]
    public class PopUpSprite
    {
        public PigType pigType;
        public Sprite sprite;
    }

    [SerializeField] private Canvas UiPopUp;
    public List<PopUpSprite> sprites;

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
        UiPopUp.transform.GetChild(0).transform.rotation = Quaternion.AngleAxis(180, UiPopUp.transform.GetChild(0).transform.right) * UiPopUp.transform.GetChild(0).transform.rotation;
    }

    public void Enable(PigType playerType)
    {
        foreach (PopUpSprite sprite in sprites)
        {
            if (playerType == sprite.pigType)
                UiPopUp.transform.GetChild(0).GetComponent<Image>().sprite = sprite.sprite;
        }
        UiPopUp.enabled = true;
    }

    public void Disable()
    {
        UiPopUp.enabled = false;
    }

    public bool CanEnable(PigType playerType)
    {

        foreach(PopUpSprite sprite in sprites)
        {
            if (playerType == sprite.pigType)
                return true;
        }

        return false;
    }
}
