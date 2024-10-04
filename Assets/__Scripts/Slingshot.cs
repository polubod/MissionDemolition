using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Slingshot : MonoBehaviour
{
    // fields set in the Unity Inspector pane
    [Header("Inscribed")] // a
    public GameObject projectilePrefab;
    public float velocityMult = 10f; // a
    public GameObject projLinePrefab;

    // fields set dynamically
    [Header("Dynamic")] // a
    public GameObject launchPoint;
    public Vector3 launchPos; // b
    public GameObject projectile; // b
    public bool aimingMode; // b

    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint"); // a
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive( false ); // b
        launchPos = launchPointTrans.position; // c

    }

   void OnMouseEnter() {
        //print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive( true );
    }
    void OnMouseExit() {

        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive( false );
    }
    void OnMouseDown() { // d
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        // Instantiate a Projectile
        projectile = Instantiate( projectilePrefab ) as GameObject;
        // Start it at the launchPoint
        projectile.transform.position = launchPos;
        // Set it to isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }
    
    void Update() {
        // If Slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return; // b
        // Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition; // c
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint( mousePos2D );

        // Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D -launchPos;
        // Limit mouseDelta to the radius of the Slingshot SphereCollider // d
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if ( Input.GetMouseButtonUp(0) ){ // e
            // The mouse has been released
            aimingMode = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;

            FollowCam.SWITCH_VIEW( FollowCam.eView.slingshot );
            FollowCam.POI = projectile;
            // Add a ProjectileLine to the Projectile
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.SHOT_FIRED();
        }
    }



}
