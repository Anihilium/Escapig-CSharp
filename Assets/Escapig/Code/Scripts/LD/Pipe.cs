using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    private CameraMovement cameraMovement;
    private List<GameObject> pipePoints;
    [SerializeField] private float SpeedThroughPipe;
    [SerializeField] private GameObject pipePointsObject;



    // Start is called before the first frame update
    void Start()
    {
        cameraMovement = Camera.main.transform.GetComponent<CameraMovement>();
        pipePoints = new List<GameObject>();

        for (int i = 0; i < pipePointsObject.transform.childCount; i++)
        {
            pipePoints.Add(pipePointsObject.transform.GetChild(i).gameObject);
        }
    }

    public void EnterPipe(Vector3 EnterPosition)
    {
        if(Vector3.Distance(EnterPosition, pipePoints[0].transform.position) > Vector3.Distance(EnterPosition, pipePoints[pipePoints.Count-1].transform.position))
        {
            pipePoints.Reverse();
            cameraMovement.GoThroughPipe(pipePoints, SpeedThroughPipe);
        }
        else
        {
            cameraMovement.GoThroughPipe(pipePoints, SpeedThroughPipe);
        }
    }
}
