using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ThirdPersonMovement : MonoBehaviour
{
    //motor that drives character
    public CharacterController controller;
    public Animator anim;
    public Transform cam;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    float turnSmoothVelocity;
    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
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
    }

    // this script pushes all rigidbodies that the character touches
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        Rigidbody body = hit.collider.attachedRigidbody;

        if (hit.gameObject.CompareTag("Collectible"))
        {
            Debug.Log("Chomp");
            hit.transform.parent = this.gameObject.transform;
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Interaction/ItemCollect", GetComponent<Transform>().position);
            hit.transform.parent = GameObject.Find("TrueBackpack").transform;
            hit.transform.localPosition = new Vector3(0f, 0f, 0f);
            body.isKinematic = false;
            body.useGravity = true;
            //hit.transform.position = this.gameObject.transform.position + new Vector3(5f, 2f, 0f);

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
