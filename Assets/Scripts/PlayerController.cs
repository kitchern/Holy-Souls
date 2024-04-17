using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Animator swordAnimator; 
    Vector3 input;
    Vector3 moveDirection;

    public float jumpHeight = 10;
    public float gravity = 9.81f;
    public float airControl = 10;
    public float speed = 10;

    // Dash variables
    public float dashSpeed = 5;
    public float dashDuration = 0.1f;
    public float dashCooldown = 1.0f;
    private bool canDash = false;
    private float dashTimeLeft;
    private float lastDash = -100f;
    public AudioClip slashFX;
    public AudioClip jumpFX;
    public GameObject shield;
    public GameObject shieldAsset;
    public Image tpIcon;

    public float tpDistance = 5f;
    public float tpCoolDown = 3f;
    bool isTPCoolDown;
    public bool isLevel3 = false;

    private bool isAttacking = false;


    void Start()
    {
        print (SceneManager.GetActiveScene().name.ToString());
        if(SceneManager.GetActiveScene().name.ToString() == "Level3")
        {
            isLevel3 = true;
        }
        controller = GetComponent<CharacterController>();
        // Find the DemonicSword child object and get its Animator component
        Transform swordTransform = transform.Find("Sword");
        if (swordTransform != null)
        {
            swordAnimator = swordTransform.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("DemonicSword not found as a child of PlayerController. Please check the hierarchy.");
        }
        isTPCoolDown = false;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        input *= speed;

        if (controller.isGrounded)
        {
            moveDirection = input;

            if (Input.GetButton("Jump"))
            {
                AudioSource.PlayClipAtPoint(jumpFX, transform.position);
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else
            {
                moveDirection.y = 0.0f;
            }
        }
        else
        {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        // Dash implementation
        if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDash + dashCooldown)
        {
            dashTimeLeft = dashDuration;
            lastDash = Time.time;
        }

        if (dashTimeLeft > 0)
        {
            moveDirection += input * dashSpeed;
            dashTimeLeft -= Time.deltaTime;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        // Trigger attack animation on left click
        if (Input.GetMouseButtonDown(0)) // Check for left mouse button press
        {
            AudioSource.PlayClipAtPoint(slashFX, transform.position);
            if (swordAnimator != null)
            {
                swordAnimator.SetTrigger("Attack");
                swordAnimator.SetTrigger("NotAttacking");
            }
            else
            {
                Debug.LogWarning("Attempted to trigger attack, but no swordAnimator is set.");
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            shield.SetActive(true);
            GameObject shieldAssert = Instantiate(shieldAsset);
            shieldAssert.transform.position = transform.position;
            shieldAssert.transform.position -= new Vector3(0, 3.5f, 0); 
            Debug.LogWarning("Shield On");
        }

        if(Input.GetKeyDown(KeyCode.T) && !isTPCoolDown && isLevel3)
        {
            Vector3 moveDirection = transform.forward * tpDistance;
            controller.Move(moveDirection);
            isTPCoolDown = true;
            Invoke("resetTP", tpCoolDown);
            Color dimColor = tpIcon.color;
            dimColor.a = 0.25f;
            tpIcon.color = dimColor;
        }
    }

    // Method to enable dash ability when collectible is picked up
    public void EnableDash()
    {
        canDash = true;
    }

    public void resetTP()
    {
        isTPCoolDown = false;
        Color fullColor = tpIcon.color;
        fullColor.a = 1f;
        tpIcon.color = fullColor;
    }
}
