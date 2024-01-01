using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class EndCredit : MonoBehaviour
{
    public Image credit;
    public Vector3 StartPosCredit;
    public Vector3 ÉndPosCredit;

    public float speed;
    float lerp;



    // Start is called before the first frame update
    void Start()
    {
        lerp = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(lerp <= 1f)
        {
            credit.rectTransform.anchoredPosition = Vector3.Lerp(StartPosCredit, ÉndPosCredit, lerp);
            lerp += speed * Time.deltaTime;
        }
        else if(lerp >= 1f)
        {
            SceneManager.LoadScene(0);
        }

    }
}
