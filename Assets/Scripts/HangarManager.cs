using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * written by Vincent Busch
 * 
 * This script facilitates numbering and labeling the hangar.
 * It also facilitates control over the hangar's lighting.
 */
public class HangarManager : MonoBehaviour
{   
    public int hangarNumber;

    [SerializeField] private TextMeshPro hangarLabel;
    [SerializeField] private List<GameObject> hangarLights = new List<GameObject>();

    private bool lightsOn;

	private void Start() {

        // set initial value for lightsOn based on the first light object in the list
        if (hangarLights.Count > 0) lightsOn = hangarLights[0].activeSelf;
	}

	// labeling

	public void UpdateHangarLabel() {
        hangarLabel.text = hangarNumber.ToString();
	}

    // lighting

    public void DeActivateLights (bool onNotOff) {
        foreach (GameObject lightObject in hangarLights) lightObject.SetActive(onNotOff);
        lightsOn = onNotOff;
	}

    public void ToggleLights() {
        if (lightsOn) DeActivateLights(false);
        else DeActivateLights(true);
	}
}
