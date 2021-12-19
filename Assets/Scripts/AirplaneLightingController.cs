using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * written by Vincent Busch
 * 
 * This script controls an airplane's lighting.
 */
public class AirplaneLightingController : MonoBehaviour
{
    [SerializeField] private List<GameObject> headlights = new List<GameObject>();
    [SerializeField] private List<GameObject> starboardNavigationLights = new List<GameObject>();
    [SerializeField] private List<GameObject> portNavigationLights = new List<GameObject>();
    [SerializeField] private float portStarboardLightSwitchingDuration = 1;

    private float portStarboardLightsLastSwitchTimestamp = 0;
    private int portStarBoardLightSwitchingState = 0;
    private bool navigationLightsActive;

    private void Update() {

        // If navigation lights are active and the port/starboard lights' switching duration passed since the last switch, switch their state.
        if (navigationLightsActive && Time.time > portStarboardLightsLastSwitchTimestamp + portStarboardLightSwitchingDuration) {

            portStarBoardLightSwitchingState++;
            if (portStarBoardLightSwitchingState > 2) portStarBoardLightSwitchingState = 0;

            switch (portStarBoardLightSwitchingState) {

                case 0: //all direction navigation lights off
                    DeActivateStarboardNavigationLights(false);
                    DeActivatePortNavigationLights(false);
                    break;

                case 1: //starboard lights on
                    DeActivateStarboardNavigationLights(true);
                    DeActivatePortNavigationLights(false);
                    break;

                case 2: //port lights on
                    DeActivateStarboardNavigationLights(false);
                    DeActivatePortNavigationLights(true);
                    break;
            }

            portStarboardLightsLastSwitchTimestamp = Time.time;
		}
    }

    // navigation lights

    private void DeActivatePortNavigationLights (bool onNotOff) {
        foreach (GameObject lightObject in portNavigationLights) lightObject.SetActive(onNotOff);
    }

    private void DeActivateStarboardNavigationLights (bool onNotOff) {
        foreach (GameObject lightObject in starboardNavigationLights) lightObject.SetActive(onNotOff);
    }

    public void ToggleNavigationLights() {

        if (navigationLightsActive) {
            DeActivateStarboardNavigationLights(false);
            DeActivatePortNavigationLights(false);
            portStarBoardLightSwitchingState = 0;
            navigationLightsActive = false;
        }
        else {
            portStarboardLightsLastSwitchTimestamp = Time.time;
            navigationLightsActive = true;
        }
    }

    public void DeActivateNavigationLights(bool onNotOff) {

        if ((onNotOff && !navigationLightsActive) || (!onNotOff && navigationLightsActive)) ToggleNavigationLights();
    }

    // headlights

    public void ToggleHeadlights() {

        if (headlights[0] && headlights[0].activeSelf) {
            foreach (GameObject lightObject in headlights) lightObject.SetActive(false);
		}
        else if (headlights[0]) {
            foreach (GameObject lightObject in headlights) lightObject.SetActive(true);
        }
        else {
            Debug.LogWarning($"{gameObject.name} is trying to toggle its headlights, but doesn't have any in the airplane lighting controller script.");
		}
	}

    public void DeActivateHeadlights(bool onNotOff) {

        if ((onNotOff && headlights[0] && !headlights[0].activeSelf) || (!onNotOff && headlights[0] && headlights[0].activeSelf)) ToggleHeadlights();
    }
}
