using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * written by Vincent Busch
 * 
 * This script is used to attach an Airplane scriptable object asset (which holds data) to a GameObject that is an airplane.
 * 
 * It also facilitates numbering and labeling the airplane.
 */

[RequireComponent(typeof(AirplaneNavigator))]
public class AirplaneManager : MonoBehaviour
{
    public int airplaneNumber;
    [SerializeField] private List<TextMeshPro> airplaneLabels = new List<TextMeshPro>();

    [SerializeField] private Airplane airplane; // the airplane object contains data about the airplane

    //own component
    private AirplaneNavigator airplaneNavigator;

	private void Awake () {
        airplaneNavigator = GetComponent<AirplaneNavigator>();
	}

    public void SetHangarPosition (Vector3 hangarPosition) {
        airplaneNavigator.hangarPosition = hangarPosition;
	}

	public void UpdateAirplaneLabels() {
        
        foreach (TextMeshPro label in airplaneLabels) {
            label.text = airplaneNumber.ToString();
		}
	}
}
