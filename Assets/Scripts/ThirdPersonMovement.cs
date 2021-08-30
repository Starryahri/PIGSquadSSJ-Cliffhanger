using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;
using UnityEngine.UI;

public class ThirdPersonMovement : MonoBehaviour
{
    //motor that drives character
    public CharacterController controller;
    public Animator anim;
    public Transform cam;
    public GameObject myVCam;
    private CinemachineFreeLook _vCamControl;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    public bool isGrounded;
    private GameObject go;

    public GameObject Player;
    public GameObject RespawnPoint;
    //fall damage stuff
    public float minSurviveFall = 2f;
    public float airTime = 0;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f; //Smoothout the rotation for player
    public float gravity = -9.81f; 
    public float jumpHeight = 3f;
    
    public float cameraZoomMult = 1;  //used in conjuction with item count to determine how far away
    private float turnSmoothVelocity;  //how fast to turn
    private Vector3 velocity;

    // Aspects for the UI for Information (Time passed and Trash collected)
    public Text UICollectibles;
    public int trashCount = 0;
    public Text UITimer;
    public float Timer = 0;
    private int minutes = 0;
    private int seconds = 0;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        go = GameObject.Find("InsideTheBackpack");
        //go = GameObject.Find("TrueBackpack");
        _vCamControl = myVCam.GetComponent<CinemachineFreeLook>();
    }
    // Update is called once per frame
    void Update()
    {
        //This looks in backpack parent and counts the items inside
        float backpackCount = go.transform.childCount;
        Debug.Log(backpackCount);

        _vCamControl.m_Orbits[1].m_Radius = 12 + (backpackCount); 

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Checks for fall time and then unparents objects
        if(!isGrounded)
        {
            airTime += Time.deltaTime;
        }

        if(isGrounded)
        {
            if(airTime > minSurviveFall)
            {
                GameObject.Find("InsideTheBackpack").transform.DetachChildren();
                //GameObject.Find("TrueBackpack").transform.DetachChildren(); will delete soon
            }
            airTime = 0;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        velocity.y += gravity * Time.deltaTime;


        if (direction.magnitude >= 0.1f)
        {
            anim.SetInteger("Run", 0);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        else{
            anim.SetInteger("Run", 1);
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.SetInteger("GoatJump", 1);
            anim.SetInteger("Run", 1);
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Movement/Jump", GetComponent<Transform>().position);
        }
        else{
            anim.SetInteger("GoatJump", 0);
        }
        controller.Move(velocity * Time.deltaTime);

        // Updates the Text in the UI Collectible Panel
        if (this.UICollectibles != null)
        {
            //this.UICollectibles.text = trashCount.ToString();
            this.UICollectibles.text = backpackCount.ToString();
        }

        // Updates the Text in the UI Timer Panel
        if (this.UITimer != null)
        {
            Timer = Time.timeSinceLevelLoad;
            minutes = (int)Timer;
            minutes = minutes / 60;
            seconds = (int)Timer;
            seconds = seconds % 60;

            if (seconds < 10)
            {
                this.UITimer.text = minutes + ":0" + seconds;
            }
            else
            {
                this.UITimer.text = minutes + ":" + seconds;
            }

        }



    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (hit.gameObject.CompareTag("Collectible"))
        {
            Debug.Log("Chomp");
            hit.transform.parent = this.gameObject.transform;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Interaction/ItemCollect", GetComponent<Transform>().position);
            hit.transform.parent = GameObject.Find("InsideTheBackpack").transform;
            //hit.transform.parent = GameObject.Find("TrueBackpack").transform;
            hit.transform.localPosition = new Vector3(0f, 0f, 0f);
            body.isKinematic = false;
            body.useGravity = true;
            //hit.transform.position = this.gameObject.transform.position + new Vector3(5f, 2f, 0f);
            
            // Keeps track of trash collected
            trashCount += 1;
        }

        if (hit.gameObject.CompareTag("KillBox"))
        {
            Debug.Log("WHAMO");
            this.gameObject.transform.position = RespawnPoint.transform.position;
            //GameObject go = Instantiate(RespawnParticles, Player.transform.position, Quaternion.identity) as GameObject;
            //Destroy(go, 3f);
        }
        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }
    }
}
