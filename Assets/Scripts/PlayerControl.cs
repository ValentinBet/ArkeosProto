using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float aerialSpeed = 10f;
    public float groundSpeed = 6f;
    public float runGroundSpeed = 8.5f;
    // public float gravity = -1.62f; // moon gravity
    public float jumpHeight = 3f;
    public Rigidbody rb;
    public LayerMask groundLayer;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.4f;

    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private Transform playerTransform;

    [Header("Gravity Zone properties")]
    public float gravZoneRotateTime = 0.5f;
    public GravityZone gravityZone;
    public bool isOnGravityZone = false;
    public bool isSettingGravZoneRotation = false;
    private Quaternion gravZoneRotation;
    [Space(8)]

    private Vector3 lastForceTake;
    private Vector3 velocity;
    private float verticalRotation;
    private float mouseX;
    private float mouseY;
    private float mouseZ;
    private Vector3 movement;

    private bool isGrounded;
    public Transform ground;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
        playerTransform = this.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
        CheckIsGrounded();
        RotationControl();

        if (isSettingGravZoneRotation)
        {
            if (isOnGravityZone)
            {
                playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, gravZoneRotation, Time.deltaTime * 10);
            }
            else
            {
                playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.Euler(0, 90, 0), Time.deltaTime * 3);
            }
        }
        else
        {
            GroundMovementControl();
            AerialMovementControl();


        }

        if (isOnGravityZone)
        {
            rb.AddForce(-transform.up * 50);
        }


    }

    private void CheckIsGrounded()
    {

        if (Physics.Raycast(groundCheck.position, -transform.up, groundCheckDistance, groundLayer))
        {

            if (!isGrounded)
            {
                print("grounded");
            }

            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, 0), Time.deltaTime * 6);

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded)
        {
            Collider[] grounds = Physics.OverlapSphere(groundCheck.position, groundCheckDistance, groundLayer);

            ground = grounds[0].transform;

        }
    }

    private void RotationControl()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        playerTransform.Rotate(Vector3.up * mouseX);
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
        mainCameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);


    }

    private void GroundMovementControl()
    {
        float inputX = 0;
        float inputZ = 0;

        if (!isGrounded)
        {
            inputX = Input.GetAxis("Horizontal");
            inputZ = Input.GetAxis("Vertical");
        }
        else
        {
            inputX = Input.GetAxisRaw("Horizontal");
            inputZ = Input.GetAxisRaw("Vertical");
        }


        if (inputX > 0)
        {
            UIManager.Instance.SetRightForce(inputX);
        }
        else
        {
            UIManager.Instance.SetRightForce(0);
        }

        if ((inputX < 0))
        {
            UIManager.Instance.SetLeftForce(-inputX);
        }
        else
        {
            UIManager.Instance.SetLeftForce(0);
        }

        if (inputZ > 0)
        {
            UIManager.Instance.SetFrontForce(inputZ);
        }
        else
        {
            UIManager.Instance.SetFrontForce(0);
        }
        if (inputZ < 0)
        {
            UIManager.Instance.SetBackForce(-inputZ);
        }
        else
        {
            UIManager.Instance.SetBackForce(0);
        }


        movement = transform.right * inputX + transform.forward * inputZ;

        if (!isGrounded)
        {
            rb.AddForce(movement * aerialSpeed);
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity += jumpHeight * transform.up;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.MovePosition(transform.position + (movement * Time.deltaTime * runGroundSpeed));
            }
            else
            {
                rb.MovePosition(transform.position + (movement * Time.deltaTime * groundSpeed));
            }
        }

    }

    private void AerialMovementControl()
    {
        if (!isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                rb.AddForce(transform.up * aerialSpeed);
                UIManager.Instance.SetUpForce(1);
            }
            else
            {
                UIManager.Instance.SetUpForce(0);
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                rb.AddForce(-transform.up * aerialSpeed);
                UIManager.Instance.SetDownForce(1);
            }
            else
            {
                UIManager.Instance.SetDownForce(0);
            }
        }
    }


    public void EnterGravityZone(GravityZone gravityZone)
    {
        this.gravityZone = gravityZone;
        gravZoneRotation = gravityZone.parent.transform.rotation;
        isOnGravityZone = true;
        rb.useGravity = false;
        StartCoroutine(SetRotationInGravZone());
    }


    public void ExitGravityZone(GravityZone gravityZone)
    {
        if (this.gravityZone = gravityZone)
        {
            rb.useGravity = true;
            this.gravityZone = null;
            isOnGravityZone = false;
            StartCoroutine(SetRotationInGravZone());
        }
    }

    IEnumerator SetRotationInGravZone()
    {
        isSettingGravZoneRotation = true;
        yield return new WaitForSeconds(gravZoneRotateTime);
        isSettingGravZoneRotation = false;
    }

}
