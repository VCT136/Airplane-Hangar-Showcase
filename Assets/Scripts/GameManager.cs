using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * written by Vincent Busch
 * 
 * The GameManager numbers and labels the hangars and aircraft.
 */
public class GameManager : MonoBehaviour
{
    [SerializeField] private List<AirplaneManager> aircraft = new List<AirplaneManager>();
    [SerializeField] private List<HangarManager> hangars = new List<HangarManager>();
    
    [Header("Parking Status")]
    [SerializeField] private TextMeshProUGUI parkingStatusIndicator;
    [SerializeField] private Color parkingStatusIndicatorNotParkedColor;
    [SerializeField] private Color parkingStatusIndicatorParkingColor;
    [SerializeField] private Color parkingStatusIndicatorParkedColor;
    [Tooltip("frequency at which the parking status is checked (Hz)")]
    [SerializeField] private float parkCheckFrequency;

    private bool aircraftParking;                   // if the aircraft should park
    private bool aircraftParked;                    // if the aircraft are parked
    private float lastParkingStatusCheckTimestamp;  // when the parking status was last checked

    [Header("Debug")]
    [SerializeField] private bool debugLog;
    [SerializeField] private bool debugParkAllAircraft;
    [SerializeField] private bool debugUnparkAllAircraft;

    void Start() {

        //number and label aircraft
        int airplaneCounter = 0;
        foreach (AirplaneManager airplane in aircraft) {
            airplane.airplaneNumber = airplaneCounter;
            airplane.UpdateAirplaneLabels();
            airplaneCounter++;
		}

        //number and label hangars
        int hangarCounter = 0;
        foreach (HangarManager hangar in hangars) {
            hangar.hangarNumber = hangarCounter;
            hangar.UpdateHangarLabel();
            hangarCounter++;
        }

        //link matching hangars to aircraft
        foreach (AirplaneManager airplane in aircraft)
		{
            foreach (HangarManager hangar in hangars)
			{
                if (airplane.airplaneNumber == hangar.hangarNumber) {
                    airplane.SetHangarPosition(hangar.transform.position);
                    if (debugLog) Debug.Log($"{airplane.name} (#{airplane.airplaneNumber}) is now linked to {hangar.name} (#{hangar.hangarNumber})");
                }
			}
        }
    }

	private void Update() {

        //check parking status
        if (Time.time > lastParkingStatusCheckTimestamp + parkCheckFrequency) {
            aircraftParked = GetParkingStatus();
            UpdateParkingStatusIndicator();
        }

        //debug
        if (debugParkAllAircraft) {
            ParkAllAircraft();
            debugParkAllAircraft = false;
		}
        if (debugUnparkAllAircraft) {
            UnparkAllAircraft();
            debugUnparkAllAircraft = false;
        }
    }

    //parking

	public void ParkAllAircraft() {
        foreach (AirplaneManager airplane in aircraft) {
            airplane.GetComponent<AirplaneNavigator>().BeginApproachingHangar();
		}
        aircraftParking = true;
	}

    public void UnparkAllAircraft() {
        foreach (AirplaneManager airplane in aircraft) {
            airplane.GetComponent<AirplaneNavigator>().Unpark();
        }
        aircraftParking = false;
    }

    public void ToggleAircraftParking() {
        if (aircraftParking) {
            UnparkAllAircraft();
		}
        else {
            ParkAllAircraft();
		}
	}

    private bool GetParkingStatus() {
        lastParkingStatusCheckTimestamp = Time.time;
        if (aircraftParking) {
            foreach (AirplaneManager airplane in aircraft) {
                if (!airplane.GetComponent<AirplaneNavigator>().parked) return false;
            }
            return true;
        }
        else return false;
    }

    private void UpdateParkingStatusIndicator() {
        if (parkingStatusIndicator) {
            if (!aircraftParking) {
                parkingStatusIndicator.text = "Aircraft Not Parked";
                parkingStatusIndicator.color = parkingStatusIndicatorNotParkedColor;
            }
            else if (!aircraftParked) {
                parkingStatusIndicator.text = "Aircraft Parking";
                parkingStatusIndicator.color = parkingStatusIndicatorParkingColor;
            }
            else {
                parkingStatusIndicator.text = "Aircraft Parked";
                parkingStatusIndicator.color = parkingStatusIndicatorParkedColor;
            }
        }
        else Debug.LogWarning($"{gameObject.name} is missing a parking status indicator");
	}

    //lighting

    public void DeActivateAllHeadlights (bool onNotOff) {
        foreach (AirplaneManager airplane in aircraft) airplane.GetComponent<AirplaneLightingController>().DeActivateHeadlights(onNotOff);
	}

    public void ToggleAllHeadlights() {
        foreach (AirplaneManager airplane in aircraft) airplane.GetComponent<AirplaneLightingController>().ToggleHeadlights();
    }

    public void DeActivateAllNavigationLights(bool onNotOff) {
        foreach (AirplaneManager airplane in aircraft) airplane.GetComponent<AirplaneLightingController>().DeActivateHeadlights(onNotOff);
    }

    public void ToggleAllNavigationLights() {
        foreach (AirplaneManager airplane in aircraft) airplane.GetComponent<AirplaneLightingController>().ToggleNavigationLights();
    }

    public void DeActivateAllHangarLights (bool onNotOff) {
        foreach (HangarManager hangar in hangars) hangar.DeActivateLights(onNotOff);
	}

    public void ToggleAllHangarLights() {
        foreach (HangarManager hangar in hangars) hangar.ToggleLights();
    }

    public void ToggleAllLights() {
        ToggleAllNavigationLights();
        ToggleAllHeadlights();
        ToggleAllHangarLights();
	}
}