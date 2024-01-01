using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : Activables
{
    List<Quaternion> penDoorRotations = new List<Quaternion>();
    FMODUnity.StudioEventEmitter[] emitters;
    //FMODUnity.StudioEventEmitter emitter;

    List<ParticleSystem> particleSystems = new List<ParticleSystem>();
    [SerializeField] private GameObject pigsInCage;


    // Start is called before the first frame update
    void Start()
    {
        Toggleable = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            penDoorRotations.Add(transform.GetChild(i).rotation);
            particleSystems.Add(transform.GetChild(i).Find("Smoke_1").GetComponent<ParticleSystem>());
        }

        emitters = GetComponents<FMODUnity.StudioEventEmitter>();
    }

    public override void Activate(bool p_on)
    {
        if (Toggleable || alreadyActivated)
            return;

        alreadyActivated = true;

        emitters[0].Play();
        emitters[1].Play();

        foreach(ParticleSystem p in particleSystems)
            p.Play();

        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).GetComponent<Collider>());

        curTime = Time.deltaTime;

        if (pigsInCage)
            LiberatePigs();

        Destroy(transform.gameObject);
    }

    private void Update()
    {
        //if (curTime <= 0f || animationEnded)
        //    return;
        
        //for(int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).rotation = Quaternion.Slerp(penDoorRotations[i], Quaternion.Euler(90f, penDoorRotations[i].eulerAngles.y, 0f), curTime / LerpTime);
        //    if (Quaternion.Angle(transform.GetChild(0).rotation, Quaternion.Euler(90f, penDoorRotations[i].eulerAngles.y, 0f)) < 1f)
        //    {
        //        transform.GetChild(i).rotation = Quaternion.Euler(0f, penDoorRotations[i].eulerAngles.y, 0f);
        //        animationEnded = true;
        //    }
        //}
        //curTime += Time.deltaTime;
    }

    public void LiberatePigs()
    {
        GameObject pigsLiberated = pigsInCage.transform.GetChild(0).gameObject;
        GameObject pigsNoLiberated = pigsInCage.transform.GetChild(1).gameObject;

        pigsNoLiberated.SetActive(false);
        pigsLiberated.SetActive(true);
    }
}
