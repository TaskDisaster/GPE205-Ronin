using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Component for creating noises that AI or players can detect.
public class NoiseMaker : MonoBehaviour
{
    #region Variables
    public float volumeDistance;    // Distance of noise
    public float decayPerFrame;     // How much the volume decreases per frame
    #endregion

    public void Start()
    {

    }

    public void Update()
    {
        // Decay volume so I don't have to
        if (volumeDistance > 0)
        {
            volumeDistance -= decayPerFrame;
        }
    }


}
