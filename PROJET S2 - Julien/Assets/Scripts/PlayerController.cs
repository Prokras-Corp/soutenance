using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public CharacterController cc;
    public Animator anim;

    AudioManager am;

    //Movement Variables
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float sprintFactor = 1.75f;
    
    //Ground Checking Variables
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask noRay;
    bool isGrounded;

    //Gravity vector
    Vector3 velocity;

    //Mouse Look
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public Transform cameraHolder;
    private float verticalRotation = 0f;

    PhotonView PV;

    //Grapple Vars
    public enum State
    {
        Normal,
        HookshotFlyingPlayer,
    }
    public State state;
    private Vector3 HookshotPosition;

    [SerializeField] GameObject torch;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        PV = GetComponent<PhotonView>();
        am = GetComponent<AudioManager>();

        //Grapple stuff
        state = State.Normal;
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        if(!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(cc);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;
        //Move();
        //Look();

        //Grapple stuff
        switch(state)
        {
            default:
            case State.Normal:
                CharacterLook();
                CharacterMovement();
                HandleHookshotStart();
                HandleBracelet();
                HandleTorch();
                HandleAnim();
                break;
            case State.HookshotFlyingPlayer:
                CharacterLook();
                HandleHookshotMovement();
                HandleBracelet();
                HandleTorch();
                HandleAnim();
                break;
        }
    }

    //player movement
    void CharacterMovement()
    {
        //Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask)  || Physics.CheckSphere(groundCheck.position, groundDistance, noRay);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Movement Inputs and actions
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        if (Input.GetKey(KeyCode.LeftShift))
            cc.Move(move.normalized * speed * sprintFactor * Time.deltaTime);
        else
            cc.Move(move.normalized * speed * Time.deltaTime);


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            am.Play("footsteps.wav");
        }
    }

    void CharacterLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraHolder.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void HandleHookshotStart()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Physics.Raycast(cameraHolder.transform.position, cameraHolder.transform.forward, out RaycastHit raycastHit))
            {
                HookshotPosition = raycastHit.point;
                state = State.HookshotFlyingPlayer;
            }
        }
    }

    private void HandleHookshotMovement()
    {
        Vector3 HookshotDir = (HookshotPosition - cc.transform.position).normalized;

        float HookshotSpeedMin = 20f;
        float HookshotSpeedMax = 40f;

        float HookshotSpeed = Mathf.Clamp(Vector3.Distance(cc.transform.position, HookshotPosition), HookshotSpeedMin, HookshotSpeedMax);
        float SpeedMultiplier = 3f;

        cc.Move(HookshotDir * HookshotSpeed * SpeedMultiplier * Time.deltaTime);

        float ReachedHookshotPositionDistance = 4f;
        if(Vector3.Distance(cc.transform.position, HookshotPosition) < ReachedHookshotPositionDistance)
        {
            state = State.Normal;
            velocity.y = -2f;
        }
    }

    void HandleBracelet()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameObject gate = GameObject.FindGameObjectWithTag("gate");
            gate.transform.eulerAngles = new Vector3(gate.transform.eulerAngles.x, gate.transform.eulerAngles.y + 180, gate.transform.eulerAngles.z);
        }
    }

    void HandleTorch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (torch.activeInHierarchy)
            {
                torch.SetActive(false);
            }
            else
            {
                torch.SetActive(true);
            }
        }
    }

    void HandleAnim()
    {
        if (state == State.HookshotFlyingPlayer)
        {
            anim.SetBool("IsGrounded", true);
            anim.SetBool("Walking", false);
            anim.SetBool("Running", false);
            anim.SetBool("Grappin", true);
        }
        else
        {
            anim.SetBool("Grappin", false);
            if (!isGrounded)
            {
                anim.SetBool("Walking", false);
                anim.SetBool("Running", false);
                anim.SetBool("IsGrounded", false);
            }
            else
            {
                anim.SetBool("IsGrounded", true);
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                {
                    anim.SetBool("Walking", true);
                    anim.SetBool("Running", Input.GetKey(KeyCode.LeftShift));
                }
                else
                {
                    anim.SetBool("Walking", false);
                    anim.SetBool("Running", false);
                }
            }
        }
    }
}

//if attack >> am.Play("punch.wav");
