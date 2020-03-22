using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float speed = 10f;
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

        if (mainAttractor != null)
        {
            UIManager.Instance.SetGravity(mainAttractor.gravityToPlayer, mainAttractor.name);
        }
    }

    void Update()
    {
        RotationControl();
        GroundMovementControl();
        AerialMovementControl();
    }

    private void CheckIsGrounded()
    {
         isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);

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

        //verticalRotation -= mouseY;

        //if (isGrounded)
        //{
        //    verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);
        //    mainCameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        //}

        if (Input.GetKey(KeyCode.E))
        {
            mouseZ = mouseX;
            mouseX = 0;
        }

        playerTransform.Rotate(-1 * mouseY, 1 * mouseX, -1 * mouseZ);



    }

    private void GroundMovementControl()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f);
        }

        movement = transform.right * inputX + transform.forward * inputZ;

        rb.AddForce(movement * speed);
    }

    private void AerialMovementControl()
    {
        if (!isGrounded)
        {
            // mainCameraTransform.transform.rotation = Quaternion.Lerp(mainCameraTransform.transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime * 5);

            if (Input.GetKey(KeyCode.A))
            {
                rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.deltaTime * 3);
            }

            if (Input.GetButton("Jump"))
            {
                rb.AddForce(transform.up * speed);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddForce(-transform.up * speed);
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
