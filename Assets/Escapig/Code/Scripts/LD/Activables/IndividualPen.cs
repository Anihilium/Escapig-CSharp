using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualPen : Activables
{
    FMODUnity.StudioEventEmitter[] emitters;
    [SerializeField] private PigType pigToLiberate;
    PlayerManager playerManager;


    // Start is called before the first frame update
    void Start()
    {
        Toggleable = false;

        emitters = GetComponents<FMODUnity.StudioEventEmitter>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    public override void Activate(bool p_on)
    {
        if (Toggleable || alreadyActivated)
            return;

        alreadyActivated = true;

        emitters[0].Play();
        emitters[1].Play();

        playerManager.FreePig(pigToLiberate);
        StartCoroutine(DestroyCoroutine());

    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
