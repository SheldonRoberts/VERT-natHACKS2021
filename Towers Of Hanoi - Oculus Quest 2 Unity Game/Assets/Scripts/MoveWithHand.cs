using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithHand : MonoBehaviour
{
    public Camera viewCamera;
    private GameObject lastGazedUpon;
    public Transform ControllerPosition;
    public UdpSocket socket;
    public SimpleGetData SimpleGet;
    public float LiftHeight = 1.2f;
    public float BaseHeight = 0.9f;
    public float LiftSpeed = 0.01f;
    public float CorrectionSpeed = 0.01f;
    public List<float> polePositions;
    public List<GameObject> poleList;
    private bool FocusInitiated = false;
    private bool FocusOnSelf = false;
    public static int MovesTaken = 0;

    public Pile pile;
    public GameMain gameMain;

    void Start()
    {

        poleList = gameMain.getPoleList();
    }


    void Update()
    {


        if (SimpleGet.GetData() == "True") // Checks if the user is focusing
        {
            if (!FocusInitiated) // This runs when focus is first initiated, to check what object the user is looking at
            {
                FocusInitiated = true;

                // Raycast out from the front of the VR camera view to find a users gaze
                Ray gazeRay = new Ray(viewCamera.transform.position, viewCamera.transform.rotation * Vector3.forward);
                RaycastHit hit;
                if (Physics.Raycast(gazeRay, out hit, Mathf.Infinity))
                {
                    lastGazedUpon = hit.transform.gameObject;
                    
                    // Store the gameObject of the first thing the ray hits
                }

                // Check if self (the object this script is attached to) is a parent of the object the ray hit
                if (lastGazedUpon && lastGazedUpon.transform.IsChildOf(transform))
                {
                    MovesTaken++;
                    FocusOnSelf = true;
                }
            }
            if (FocusOnSelf) // if the user is focusing and the focus is targeted on this object
            {
                foreach (var pole in poleList)
                {
                    pole.GetComponent<Pile>().removeDisc(gameObject);
                }
                float y = transform.position.y;
                if (transform.position.y < LiftHeight) // Lift the object to its lifted height
                {
                    y = transform.position.y + LiftSpeed;
                }// set the y to lifted height and the x to match the controller position
                transform.position = new Vector3(ControllerPosition.position.x, y, transform.position.z);
            }
        }
        else // When there is no focus on this object
        {
            FocusOnSelf = false;
            FocusInitiated = false;
            lastGazedUpon = null;
            float y = transform.position.y;
            float x = transform.position.x;
            foreach (var pole in poleList)
            {
                var gap = transform.position.x - pole.transform.position.x;
                if (gap > -0.1 && gap < 0.1)
                {

                    pole.GetComponent<Pile>().addDisc(gameObject);
                    BaseHeight = pole.GetComponent<Pile>().getBaseHeight(gameObject);
                }
            }


            if (transform.position.y > BaseHeight) // Lower the object back down to its original height
            {
                y = transform.position.y - LiftSpeed;

                // If the ring was dropped close, but not exactly above a pole, shift it over
                foreach (var pole in poleList)
                {
                    var gap = transform.position.x - pole.transform.position.x;
                    if (gap > 0 && gap < 0.1)
                    {
                        x = transform.position.x - CorrectionSpeed;

                    }
                    else if (gap < 0 && gap > -0.1)
                    {
                        x = transform.position.x + CorrectionSpeed;

                    }
                }

            }
            transform.position = new Vector3(x, y, transform.position.z);
        }



    }
}
