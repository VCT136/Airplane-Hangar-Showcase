using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * written by Vincent Busch
 * 
 * This script is responsible for an airplane's movement utilizing the NavMeshAgent component.
 */
[RequireComponent(typeof(NavMeshAgent))]
public class AirplaneNavigator : MonoBehaviour
{
    [Header("Random Destination Area")]
    [Tooltip("postion of the corner of the random destination area with the lowest coordinate values")]
    [SerializeField] Vector3 randomDestinationsAreaMin;
    [Tooltip("postion of the corner of the random destination area with the highest coordinate values")]
    [SerializeField] Vector3 randomDestinationsAreaMax;
    [Tooltip("distance to a random destination that must be reached, in order to allow picking a new random destination")]
    [SerializeField] float randomDestinationApproachDistanceTolerance = 0.1f;

    [Header("Automated - Do not change in inspector!")]
    [Tooltip("the airplane's hangar position")]
    public Vector3 hangarPosition;
    [Tooltip("if the airplane is parked")]
    public bool parked;

    private bool approachingDestination;
    private bool parking;

    private NavMeshAgent navAgent;

    [Header("Debug")]
    [SerializeField] private Vector3 debugDestination;
    [SerializeField] private bool debugBeginApproach;

    void Awake() {

        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update() {

        //check for arrival
        if ((transform.position - navAgent.destination).magnitude < randomDestinationApproachDistanceTolerance) {
            approachingDestination = false;
            if (parking) {
                parked = true;
                parking = false;
			}
        }

        //set a new random destination and approach it if no other is currently being approached, and the airplane is not parked
        if (!approachingDestination && !parked) {
            Vector3 randomDestination = new Vector3(
                Random.Range(randomDestinationsAreaMin.x, randomDestinationsAreaMax.x),
                Random.Range(randomDestinationsAreaMin.y, randomDestinationsAreaMax.y),
                Random.Range(randomDestinationsAreaMin.z, randomDestinationsAreaMax.z)
            );
            BeginApproachingDestination(randomDestination);
		}

        //debug
        if (debugBeginApproach) {
            BeginApproachingDestination(debugDestination);
            debugBeginApproach = false;
		}
	}

    public void BeginApproachingDestination (Vector3 destination) {

        navAgent.destination = destination;
        approachingDestination = true;
	}

    public void BeginApproachingHangar() {
        BeginApproachingDestination(hangarPosition);
        parking = true;
	}

    // stop being parked
    public void Unpark() {
        parked = false;
	}
}
