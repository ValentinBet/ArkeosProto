using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{
    /// <summary>
    /// Ce code est uniquement crée à but de prototypage 
    /// Certaine accesibilité de propriétés et de méthodes de celui-ci sont à revoir
    /// De plus de sa "proprété" sémantique et structurelle 
    /// </summary>
    /// <remarks>
    /// Cette classe est le moteur principal de ce prototype
    /// </remarks>
    public Rigidbody rb;
    public LayerMask groundLayer;

    public float mouseSensitivity = 100f;
    public float aerialSpeed = 10f;
    public float groundSpeed = 6f;
    public float runGroundSpeed = 8.5f;
    public float jumpHeight = 3f;
    public float jetPackWaitTimeAfterJump = 0.2f;
    public Light jetPackLightFeedback;

    [Space(8)]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform UpCheck;
    [SerializeField] private float groundCheckDistance = 0.3f;
    [SerializeField] private Transform mainCameraTransform;
    [SerializeField] private Transform playerTransform;

    [Header("JetPack properties")]
    public float jetPackFuel = 100f;
    public float jetPackRegen = 1f;
    public float jetPackHorizontalUsage = 1f;
    public float jetPackVerticalUsage = 2f;
    public float baseJetpackLightIntensity = 1f;
    private float jetPackUsageStack;

    [Header("Gravity Zone properties")]
    //public float gravZoneEnterRotateTime = 0.5f;
    //public float gravZoneExitRotateTime = 2f;
    public float rotateSpeedEnterGravZone = 10;
    public float rotateSpeedExitGravZone = 15;
    public float speedDividerWhenEntered = 4f;
    public GravityZone gravityZone;
    public bool isOnGravityZone = false;
    private bool isSettingGravZoneRotation = false;
    private bool isSettingExitGravZoneRotation = false;
    private Quaternion gravZoneRotation;
    [HideInInspector] public float gravZonePower = 0f; // Set when enter a grav zone
    [Space(8)]

    private Vector3 lastForceTake;
    private Vector3 velocity;
    private float verticalRotation;
    private float mouseX;
    private float mouseY;
    private float mouseZ;
    private Vector3 movement;
    // GravZoneRotation
    private Vector3 closestPoint = Vector3.zero;
    private Vector3 gravZoneDirection = Vector3.zero;
    private Quaternion targetRotation;
    // <<
    private bool canUseJetPack = true;
    private bool isGrounded;
    private Transform ground;
    private float jetPackMaxFuel;

    private void Start()
    {
        jetPackMaxFuel = jetPackFuel;
        mainCameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
        CheckIsGrounded();


        if (!isOnGravityZone)
        {
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.Euler(0, playerTransform.rotation.eulerAngles.y, 0), Time.deltaTime * rotateSpeedExitGravZone);
        }

        AerialMovementControl();
        GroundMovementControl();
        RotationControl();

        if (isOnGravityZone)
        {
            closestPoint = gravityZone.parentCollider.ClosestPoint(UpCheck.position);
            gravZoneDirection = closestPoint - UpCheck.position;
            targetRotation = Quaternion.FromToRotation(-transform.up, gravZoneDirection) * playerTransform.rotation;
            playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotateSpeedEnterGravZone);
            rb.AddForce(-transform.up * gravZonePower);
        }

        UIManager.Instance.SetJetPackFuel(((jetPackFuel * 100) / jetPackMaxFuel) / 100);
        jetPackFuel = Mathf.Clamp(jetPackFuel, 0, jetPackMaxFuel);
        UseJetpack(jetPackUsageStack);
    }

    private void CheckIsGrounded()
    {

        if (Physics.Raycast(groundCheck.position, -transform.up, groundCheckDistance, groundLayer))
        {

            if (!isGrounded)
            {
                print("grounded");
            }

            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, 0), Time.deltaTime * 10);

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
        playerTransform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;

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

        if (inputX != 0 || inputZ != 0)
        {
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
        }

        if (isGrounded || (inputX == 0 || inputZ == 0))
        {
            jetPackFuel += jetPackRegen;
        }

        movement = transform.right * inputX + transform.forward * inputZ;

        if (!isGrounded && jetPackFuel > 0)
        {
            jetPackUsageStack += (Mathf.Abs(inputX) + Mathf.Abs(inputZ) * jetPackHorizontalUsage);
            rb.AddForce(movement * aerialSpeed);
        }
        else if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                StartCoroutine(SetCanUseJetPack());
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
            if (Input.GetButton("Jump") && canUseJetPack && jetPackFuel > 0)
            {
                jetPackUsageStack += 1 * jetPackVerticalUsage;
                rb.AddForce(transform.up * aerialSpeed);
                UIManager.Instance.SetUpForce(1);
            }
            else
            {
                UIManager.Instance.SetUpForce(0);
            }

            if (Input.GetKey(KeyCode.LeftControl) && jetPackFuel > 0)
            {
                jetPackUsageStack += 1 * jetPackVerticalUsage;
                rb.AddForce(-transform.up * aerialSpeed);
                UIManager.Instance.SetDownForce(1);
            }
            else
            {
                UIManager.Instance.SetDownForce(0);
            }
        }
    }

    private void UseJetpack(float power)
    {
        float light = 0;

        if (!isGrounded)
        {
            light += baseJetpackLightIntensity;
        }

        light += power;
        jetPackFuel -= jetPackUsageStack;
        jetPackLightFeedback.intensity = light;
        jetPackUsageStack = 0;
    }


    public void EnterGravityZone(GravityZone gravityZone)
    {
        isSettingExitGravZoneRotation = false;
        this.gravityZone = gravityZone;
        rb.velocity /= speedDividerWhenEntered;
        gravZoneRotation = gravityZone.parent.transform.rotation;
        isOnGravityZone = true;
        rb.useGravity = false;
    }


    public void ExitGravityZone(GravityZone gravityZone)
    {
        if (this.gravityZone == gravityZone)
        {
            isSettingGravZoneRotation = false;
            rb.useGravity = true;
            isOnGravityZone = false;
        }
    }


    IEnumerator SetCanUseJetPack()
    {
        canUseJetPack = false;
        yield return new WaitForSeconds(jetPackWaitTimeAfterJump);
        canUseJetPack = true;
    }

}
