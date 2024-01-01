using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{

    private GameObject hitObject;
    private CapsuleCollider capsule;

    [SerializeField] float distanceInteract = 2f;

    // Start is called before the first frame update
    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 startRaycast1 = new Vector3(transform.position.x, transform.position.y - (capsule.height / 4f), transform.position.z);
        Vector3 startRaycast2 = new Vector3(transform.position.x, transform.position.y - (capsule.height / 4f), transform.position.z) - transform.right * capsule.radius;
        Vector3 startRaycast3 = new Vector3(transform.position.x, transform.position.y - (capsule.height / 4f), transform.position.z) + transform.right * capsule.radius;

        Debug.DrawRay(startRaycast1, transform.forward * distanceInteract, Color.red);
        Debug.DrawRay(startRaycast2, transform.forward * distanceInteract, Color.red);
        Debug.DrawRay(startRaycast3, transform.forward * distanceInteract, Color.red);


        if (Physics.Raycast(startRaycast1, transform.forward, out hit, distanceInteract) || Physics.Raycast(startRaycast2, transform.forward, out hit, distanceInteract) || Physics.Raycast(startRaycast3, transform.forward, out hit, distanceInteract))
        {
            hitObject = hit.transform.gameObject;
        }
        else
        {
            hitObject = null;
        }
    }

    public GameObject GetHitObject() { return hitObject; }
}
