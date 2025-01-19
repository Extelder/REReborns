using System;
using System.Collections;
using System.Collections.Generic;
using EvolveGames;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [SerializeField] private LayerMask _checkMask;
    [SerializeField] private PlayerController _controller;
    [SerializeField] private GameObject _hintObject;

    public GameObject player;

    private Transform holdPos;
    public Transform defaultholdPos;
    public Transform trupHoldPos;

    //if you copy from below this point, you are legally required to like the video
    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from
    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index
    private int DefaultLayerNumber; //layer index

    private float _lookSpeed;

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    //MouseLookScript mouseLookScript;
    void Start()
    {
        LayerNumber =
            LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
        DefaultLayerNumber = LayerMask.NameToLayer("Default");


        _lookSpeed = _controller.lookSpeed;
        StartCoroutine(Checkking());
        //mouseLookScript = player.GetComponent<MouseLookScript>();
    }

    private Coroutine stopping;

    private IEnumerator Checkking()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
                pickUpRange, _checkMask))
            {
                if (hit.collider.TryGetComponent<IPickupable>(out IPickupable pickupable))
                {
                    if (!pickupable.CanPickup)
                    {
                        _hintObject.SetActive(false);
                        continue;
                    }

                    _hintObject.SetActive(true);
                    continue;
                }
            }

            _hintObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) //change E to whichever key you want to press to pick up
        {
            if (heldObj == null) //if currently not holding anything
            {
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
                    pickUpRange, _checkMask))
                {
                    //make sure pickup tag is attached
                    if (hit.collider.TryGetComponent<IPickupable>(out IPickupable pickupable))
                    {
                        if (!pickupable.CanPickup)
                            return;
                        pickupable.Pickuped();
                        //pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if (canDrop == true)
                {
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }

        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            RotateObject();
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true
            ) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
            {
                StopClipping();
                ThrowObject();
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            if (pickUpObj.TryGetComponent<TrupPickupable>(out TrupPickupable trupPickupable))
            {
                holdPos = trupHoldPos;
                pickUpObj.transform.parent =
                    trupHoldPos.transform;
            }
            else
            {
                holdPos = defaultholdPos;
                heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            }

            heldObj.layer = LayerNumber; //change the object layer to the holdLayer

            //make sure object doesnt collide with player, it can cause weird bugs
        }
    }

    void DropObject()
    {
        if (stopping != null)

            StopCoroutine(stopping);
        stopping = StartCoroutine(Stopping(heldObj));
        //re-enable collision with player
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
    }

    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }

    void RotateObject()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Debug.Log(_lookSpeed);
            _controller.lookSpeed = _lookSpeed;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _lookSpeed = _controller.lookSpeed;
            _controller.lookSpeed = 0;
        }

        if (Input.GetKey(KeyCode.R)) //hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating

            //disable player being able to look around
            //mouseLookScript.verticalSensitivity = 0f;
            //mouseLookScript.lateralSensitivity = 0f;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            //re-enable player being able to look around
            //mouseLookScript.verticalSensitivity = originalvalue;
            //mouseLookScript.lateralSensitivity = originalvalue;
            canDrop = true;
        }
    }

    void ThrowObject()
    {
        if (stopping != null)
            StopCoroutine(stopping);
        stopping = StartCoroutine(Stopping(heldObj));
        _controller.lookSpeed = _lookSpeed;

        //same as drop function, but add force to object before undefining it
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }

    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange =
            Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position =
                transform.position +
                new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }

    private IEnumerator Stopping(GameObject held)
    {
        yield return new WaitForSeconds(1f);
        if (held != null)
        {
            held.layer = DefaultLayerNumber;
        }
    }
}