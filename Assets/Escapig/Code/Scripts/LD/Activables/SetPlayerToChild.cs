using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerToChild : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        collision.transform.parent = transform;
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.transform.parent = null;
    }
}
