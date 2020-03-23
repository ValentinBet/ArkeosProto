using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float aerialSpeed = 10f;
    public float groundSpeed = 10f;
    // public float gravity = -1.62f; // moon gravity
    public float jumpHeight = 3f;
    public Attractor mainAttractor;
    public Rigidbody rb;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private Transform playerTransform;

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

        GroundMovementControl();
        AerialMovementControl();

        if (mainAttractor != null)
        {
            UIManager.Instance.SetGravity(mainAttractor.gravityToPlayer, mainAttractor.name);
        }
    }

    void Update()
    {
        RotationControl();
    }

    private void CheckIsGrounded()
    {

        if (Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer))
        {
            if (!isGrounded)
            {
                transform.rotation = (Quaternion.FromToRotation(-transform.up, mainAttractor.transform.position - this.transform.position)) * transform.rotation;
            }

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

            Debug.DrawRay(this.transform.position, mainAttractor.transform.position, Color.red);
        }
    }

    private void RotationControl()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (Input.GetKey(KeyCode.E))
        {
            mouseZ = mouseX;
            mouseX = Mathf.Lerp(mouseX, 0, Time.deltaTime);
        }
        else
        {
            mouseZ = Mathf.Lerp(mouseZ, 0, Time.deltaTime * 3);
        }

        if (!isGrounded)
        {
            playerTransform.Rotate(-1 * mouseY, 1 * mouseX, -1 * mouseZ);
        }
        else
        {
            verticalRotation -= mouseY;
            playerTransform.Rotate(Vector3.up * mouseX);
            verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
            mainCameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        }


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
            Vector3 jumpVel = Vector3.zero;

            if (Input.GetButtonDown("Jump"))
            {
                jumpVel = transform.up * jumpHeight;
            }

            rb.velocity =  movement * groundSpeed + jumpVel;



            if (inputX == 0 && inputZ == 0)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * 10);
            }
        }

    }

    private void AerialMovementControl()
    {
        if (!isGrounded)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.deltaTime * 3);
            }

            if (Input.GetButton("Jump"))
            {
                rb.AddForce(transform.up * aerialSpeed);
                UIManager.Instance.SetUpForce(1);
            }
            else
            {
                UIManager.Instance.SetUpForce(0);
            }

            if (Input.GetKey(KeyCode.LeftShift))
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

    public void GetAttract(Vector3 force, Attractor attractor)
    {
        if (lastForceTake.magnitude < force.magnitude && mainAttractor != attractor)
        {
            mainAttractor = attractor;
        }

        rb.AddForce(force);


    }
}
