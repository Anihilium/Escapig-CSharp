using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activables : MonoBehaviour
{
    [Tooltip("True if the elevator can go up and down")]
    public bool Toggleable = false;

    protected bool alreadyActivated = false;

    protected bool animationEnded = false;

    protected float curTime = 0f;

    [Tooltip("Time for the animation to end")]
    public float LerpTime = 1f;

    abstract public void Activate(bool p_on = true);
}
