using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public Rect playBoundary;

    // Start is called before the first frame update
    void Start()
    {
        playBoundary.center = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OutOfBounds(ControllerBase playerBase)
    {
        throw new NotImplementedException(playerBase.name + " is Out of Bounds but functionality not implemented!");
    }
}
