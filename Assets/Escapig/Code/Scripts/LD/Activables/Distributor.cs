using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distributor : Activables
{
    [SerializeField] private Transform spawnCarrotPoint;
    [SerializeField] private GameObject CarrotPrefab;

    FMODUnity.StudioEventEmitter emitter;
    [SerializeField] private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Activate(bool p_on)
    {
        RaycastHit hit;
        if(Physics.Raycast(spawnCarrotPoint.position, Vector3.down, out hit))
        {
            if (hit.transform.GetComponent<Carrot>() != null)
                return;

            Instantiate(CarrotPrefab, spawnCarrotPoint.position, Quaternion.identity);
            emitter.Play();
            animator.SetTrigger("activate");
        }
    }
}
