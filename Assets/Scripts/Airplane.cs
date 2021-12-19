using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * written by Vincent Busch
 * 
 * This scriptable object class enables the creation of Airplane type assets.
 */
[System.Serializable]
[CreateAssetMenuAttribute(fileName = "Plane", menuName = "Airplane")]
public class Airplane : ScriptableObject
{
    public enum airplaneBrand {
        Atmokart,
        Beachplane,
        Boink,
        Ceres,
        Pypair,
        Sassner
	}

    public airplaneBrand brand;
    public string type;
}
