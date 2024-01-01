using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAutomaticRotationController : MonoBehaviour
{

    [SerializeField]
    private Vector3 RotationSpeed = Vector3.up;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotationSpeed * Time.deltaTime);
    }
}
