using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    Rigidbody rb;
    CharacterController cc;

    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    public bool isDash = false;
    int staminaPoint = 3;
    float stamRacharge = 5;
    float timeRemaining = 0.5f;
    public Slider stamBar;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cc = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        stamBar.value = staminaPoint;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && staminaPoint != 0 && !isDash)
        {
            isDash = true;
        }

        if (isDash)
        {
            DashTimer();
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        //Move in Look direction and turning
        if (direction.magnitude >= 0.1f && !isDash)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (staminaPoint != 3)//If staminer is lower then 3 it will start the recharge
        {
            StaminaTimer();
        }
    }

    void DashTimer()//Forces the dash
    {
        if (timeRemaining > 0)
        {
            rb.AddForce(30 * transform.forward);
            cc.enabled = false;
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            cc.enabled = true;
            isDash = false;
            timeRemaining = 0.5f;
            staminaPoint -= 1;
        }
    }

    void StaminaTimer()//Recharges the staminer
    {
        if (stamRacharge > 0)
        {
            stamRacharge -= Time.deltaTime;
        }
        else
        {
            stamRacharge = 5f;
            staminaPoint += 1;
        }
    }
}

