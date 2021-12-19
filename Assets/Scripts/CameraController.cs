using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * written by Vincent Busch
 * 
 * This script controls the camera.
 */
public class CameraController : MonoBehaviour {

    private enum CameraView {
        defaultView,
        planeView
	}

    [Header("Default View")]
    [SerializeField] private Vector3 defaultPosition;
    [Tooltip("position the camera rotates around in default view")]
    [SerializeField] private Vector3 defaultRotationAnchor;
    [Tooltip("rotation speed in default view (deg/s)")]
    [SerializeField] private float defaultViewRotationSpeed;
    [Header("Airplane View")]
    [Tooltip("list of all airplanes accessible in plane view")]
    [SerializeField] private List<GameObject> airplanes;
    [Tooltip("offset from the airplane's transform position for the camera in plane view")]
    [SerializeField] private Vector3 planeViewOffset;
    [Tooltip("rotation speed in plane view (deg/s)")]
    [SerializeField] private float planeViewRotationSpeed;

    private Vector3 defaultViewUserPosition;                // position the camera has in default view including user changes
    private Quaternion defaultViewUserRotation;             // rotation the camera has in default view including user changes
    private int activePlane;                                // list index of the active plane for plane view
    private Transform planeTransform;                       // transform of the plane for plane view
    private CameraView cameraView = CameraView.defaultView; // active camera view
    private bool rotating;                                  // if the camera is rotating
    private bool rotatingLeft;                              // if the camera's rotation direction is left

	private void Start() {
        // initial positioning
        transform.position = defaultPosition;
        LookAtDefaultAnchor();

        // set user variables to initial values
        defaultViewUserPosition = transform.position;
        defaultViewUserRotation = transform.rotation;
	}

	private void FixedUpdate() {
		
        // camera movement
        if (rotating) {
            switch (cameraView) {

                // default view
                case CameraView.defaultView:
                    if (rotatingLeft) transform.RotateAround(defaultRotationAnchor, Vector3.up, defaultViewRotationSpeed * Time.deltaTime);
                    else transform.RotateAround(defaultRotationAnchor, Vector3.down, defaultViewRotationSpeed * Time.deltaTime);
                    LookAtDefaultAnchor();
                    defaultViewUserPosition = transform.position;
                    defaultViewUserRotation = transform.rotation;
                    break;

                // plane view
                case CameraView.planeView:
                    if (rotatingLeft) transform.Rotate(-1 * transform.up * planeViewRotationSpeed * Time.deltaTime);
                    else transform.Rotate(transform.up * planeViewRotationSpeed * Time.deltaTime);
                    break;
            }
        }
	}
    
    // rotations

    public void ReturnToInitialPosition() {

        switch (cameraView) {
            case CameraView.defaultView:
                transform.position = defaultPosition;
                LookAtDefaultAnchor();
                break;

            case CameraView.planeView:
                transform.position = planeTransform.position + planeViewOffset;
                transform.rotation = planeTransform.rotation;
                break;
		}
	}

    public void StartStopRotateLeft(bool startNotStop) {
        if (startNotStop) {
            rotating = true;
            rotatingLeft = true;
        }
        else rotating = false;
    }

    public void StartStopRotateRight(bool startNotStop) {
        if (startNotStop) {
            rotating = true;
            rotatingLeft = false;
        }
        else rotating = false;
    }

    // default view

    public void EnterDefaultView() {
        cameraView = CameraView.defaultView;
        transform.parent = null;
        transform.position = defaultViewUserPosition;
        transform.rotation = defaultViewUserRotation;
	}

    private void LookAtDefaultAnchor() {
        transform.rotation = Quaternion.LookRotation(defaultRotationAnchor - transform.position, Vector3.up);
    }

    // plane view

    public void EnterPlaneView() {
        if (airplanes[activePlane]) {
            cameraView = CameraView.planeView;
            planeTransform = airplanes[activePlane].transform;
            transform.position = planeTransform.position + planeViewOffset;
            transform.rotation = planeTransform.rotation;
            transform.parent = planeTransform;
        }
        else Debug.LogError($"{gameObject.name}'s activePlane variable is not a valid list index for its airplanes list");
    }

    public void SelectNewPlane(bool forwards) {
        if (forwards) {
            activePlane++;
            if (activePlane > airplanes.Count - 1) activePlane = 0;
        }
        else {
            activePlane--;
            if (activePlane < 0) activePlane = airplanes.Count - 1;
        }
        if (cameraView == CameraView.planeView) EnterPlaneView();
    }
}
