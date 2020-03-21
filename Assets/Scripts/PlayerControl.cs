using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float speed = 10f;
    public float gravity = -1.62f; // moon gravity
    public float jumpHeight = 3f;
    public float mass = 70f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private CharacterController controller;

    private float verticalRotation;
    private Vector3 movement;
    private Vector3 velocity;
    private bool isGrounded;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
        playerTransform = this.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        CheckIsGrounded();

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void Update()
    {
        LookControl();
        GroundMovementControl();
        AerialMovementControl();
    }

    private void CheckIsGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundLayer);    
    }

    private void LookControl()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        playerTransform.Rotate(Vector3.up * mouseX);
        mainCameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void GroundMovementControl()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        movement = transform.right * inputX + transform.forward * inputZ;
        controller.Move(movement * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void AerialMovementControl()
    {
        // Jetpack
    }

}
