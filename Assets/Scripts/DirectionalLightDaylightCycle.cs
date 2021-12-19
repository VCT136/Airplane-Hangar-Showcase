using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * written by Vincent Busch
 * 
 * This script uses a directional light to create a daylight cycle by rotating it.
 */
[RequireComponent(typeof(Light))]
public class DirectionalLightDaylightCycle : MonoBehaviour
{
    [Tooltip("speed of the daylight cycle in degrees per second")]
    [SerializeField] private float cycleSpeed;

    void Update() {

        transform.Rotate(cycleSpeed * Time.deltaTime, 0, 0);
    }
}
