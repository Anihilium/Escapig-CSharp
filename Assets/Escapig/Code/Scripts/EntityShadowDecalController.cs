using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(DecalProjector))]
public class EntityShadowDecalController : MonoBehaviour
{

    [SerializeField]
    private float MaxProjectionDistance = 2;
    [SerializeField]
    private Vector2 ProjectionSize = new Vector2(1,1);
    [SerializeField, Range(0,1)]
    private float MaxProjectionOpacitye = .8f;

    private DecalProjector m_Projector;

    // Start is called before the first frame update
    void Start()
    {
        m_Projector = GetComponent<DecalProjector>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position - Vector3.up);

        Vector3 l_proj_size = m_Projector.size;

        float l_distance_from_ground = DistanceFromGround();

        l_proj_size.x = (1 - (l_distance_from_ground / MaxProjectionDistance)) * ProjectionSize.x;
        l_proj_size.y = (1 - (l_distance_from_ground/ MaxProjectionDistance)) * ProjectionSize.y;
        
        l_proj_size.z = MaxProjectionDistance;

        m_Projector.size = l_proj_size;


        m_Projector.fadeFactor = (1 - (l_distance_from_ground / MaxProjectionDistance)) * MaxProjectionOpacitye;
    }

    private float DistanceFromGround()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (!(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, MaxProjectionDistance, layerMask)))
        {
            return MaxProjectionDistance;
        }

        return hit.distance;
    }
}
