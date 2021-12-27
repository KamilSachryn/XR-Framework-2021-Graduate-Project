using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Connection;
using MLAPI.NetworkVariable;
using MLAPI.SceneManagement;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : NetworkBehaviour
{
    /////
    bool doLog = true;
    /////

    float yVelocity = 0f;
    [Range(5f, 25f)]
    public float gravity = 1f;
    [Range(5f, 15f)]
    public float movementSpeed = 10f;
    [Range(5f, 15f)]
    public float jumpSpeed = 10f;

    Transform cameraTransform;
    Camera playerCamera;
    float pitch = 0f;
    [Range(1f, 90f)]
    public float maxPitch = 85f;
    [Range(-1f, -90f)]
    public float minPitch = -85f;
    [Range(0.5f, 5f)]
    public float mouseSensitivity = 2f;

    public float dragSpeed = 5;

    bool holdingItem = false;
    Transform heldItem = null;
    Transform heldItemParent = null;

    public GameObject networkCube;

    CharacterController cc;

    ulong clientID;

    private Animator animator;
    private void Start()
    {
        //Get client ID
        clientID = NetworkManager.Singleton.LocalClientId;

        //Local obj for referecing camera
        playerCamera = GetComponentInChildren<Camera>();
        cameraTransform = playerCamera.transform;


        //set local objects to affect ONLY the player
        if (IsLocalPlayer)
        {

            cc = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }
        else
        {
            //disable cameras for remote players
            cameraTransform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //if fell under world, reset pos
        if (this.transform.position.y < -10)
        {
            gameObject.transform.position = new Vector3(0, 1, 0);
            return;
        }
        //handle player movement
        if (IsLocalPlayer)
        {
            //unlock cursor
            if (Input.GetKeyDown("escape"))
            {
                //Screen.lockCursor = false;
                Cursor.lockState = CursorLockMode.None;
            }
            //on click, lock cursor into the game
            else if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            //if we are clicked into the game, move camera
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Look();
            }

            //handle movement
            Move();

            //invisible ray which points to middle of player's screen
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 1, Color.yellow);

            //pick up item
            if (Input.GetKeyUp("e"))
            {

                //if we are holding an item, drop it
                if (holdingItem)
                {
                    DropItem();
                    Log("Holding item, dropping");
                    holdingItem = false;
                    heldItem = null;


                }
                //if not holding an item, try to pick one up
                else if (Physics.Raycast(ray, out hit))
                {

                    Transform objHit = hit.transform;

                    Log("hit " + objHit);
                    //obj listed as interactable
                    if (objHit.tag == "movable")
                    {
                        Log("obj was movable");
                        holdingItem = true;
                        heldItem = objHit;
                        PickUp(heldItem);

                    }
                    //cant move walls/floor/etc
                    else
                    {
                        Log("obj not movable");
                    }

                }
            }
            //create new object
            else if (Input.GetKeyUp("r"))
            {
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    spawnCubeServerRpc(hit.point);
                }
            }


        }
        else
        {
            //if not local player
        }


        //set object as child of player to move it
        void PickUp(Transform t)
        {

            NetworkObject tnet = t.GetComponent<NetworkObject>();

            //take ownership of object to move it properly
            if (!tnet.IsOwner)
            {
                TakeOwnerServerRpc(tnet.NetworkObjectId, clientID);
                //tnet.ChangeOwnership(this.OwnerClientId);
            }

            heldItemParent = t.parent;
            t.SetParent(cameraTransform);
            print("held parent is " + heldItemParent);
        }

        //put object back to it's original parent
        void DropItem()
        {
            heldItem.SetParent(heldItemParent);
            heldItemParent = null;
        }


        //mouse controls
        void Look()
        {
            float xInput = Input.GetAxis("Mouse X") * mouseSensitivity;
            float yInput = Input.GetAxis("Mouse Y") * mouseSensitivity;
            transform.Rotate(0, xInput, 0);
            pitch -= yInput;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            Quaternion rot = Quaternion.Euler(pitch, 0, 0);
            cameraTransform.localRotation = rot;
        }

        //keyboard/controller controls
        void Move()
        {
            Vector3 move = Vector3.zero;
            yVelocity -= gravity * Time.deltaTime;
            move.y = yVelocity;
            cc.Move(move * Time.deltaTime);

            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (input.x > 0.01 || input.z > 0.01 || input.x < -0.01 || input.z < -0.01)
            {
                animator.SetBool("walking", true);
            }
            else
            {
                animator.SetBool("walking", false);
            }

            input = Vector3.ClampMagnitude(input, 1f);
            move = transform.TransformVector(input) * movementSpeed;



            if (cc.isGrounded)
            {
                animator.SetBool("jumping", false);
                yVelocity = -gravity * Time.deltaTime;
                //check for jump here
                if (Input.GetButtonDown("Jump"))
                {
                    yVelocity = jumpSpeed;
                    animator.SetBool("jumping", true);
                }
            }

            move.y = yVelocity;
            cc.Move(move * Time.deltaTime);

            if (cc.velocity.x == 0 && cc.velocity.z == 0)
            {
                animator.SetBool("walking", false);
            }
        }




    }

    //change the owner of a gameobject
    //this lets us move it without making the host do it
    [ServerRpc]
    public void TakeOwnerServerRpc(ulong networkID, ulong ownerID)
    {
        foreach (GameObject i in (GameObject.FindGameObjectsWithTag("movable")))
        {
            NetworkObject iNet = i.GetComponent<NetworkObject>();
            if (iNet.NetworkObjectId == networkID)
            {
                iNet.ChangeOwnership(ownerID);
            }

        }

    }

    //create a new object in the world
    //this can be changed to spawn any object,
    //even players, but they wont be controllable
    [ServerRpc]
    public void spawnCubeServerRpc(Vector3 pos)
    {
        GameObject go = Instantiate(networkCube, pos, Quaternion.identity);
        go.GetComponent<NetworkObject>().Spawn();
    }

    void Log(object msg)
    {
        if (doLog)
        {
            Debug.Log(msg);
        }
    }

    //Change scene to supplied scene
    [ServerRpc]
    public void switchSceneServerRpc(string scene)
    {
        NetworkSceneManager.SwitchScene(scene);

    }







}
