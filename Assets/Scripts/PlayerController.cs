using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject viewPoint;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float verticalRotationStore;
    [SerializeField] private bool invertLook;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private CharacterController CharCom;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float gravityMod = 2.5f;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;

    private float activeMoveSpeed;
    private Vector3 moveDir, movement;
    private Vector2 mouseInput;
    private Camera cam;

    // Start is called before the first frame update
    private void Start()
    {
        viewPoint = this.transform.GetChild(1).gameObject;
        CharCom = this.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
        this.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        verticalRotationStore += mouseInput.y;
        verticalRotationStore = Mathf.Clamp(verticalRotationStore, -60f, 60f);
        if (invertLook)
        {
            viewPoint.transform.rotation = Quaternion.Euler(verticalRotationStore, viewPoint.transform.rotation.eulerAngles.y, viewPoint.transform.rotation.eulerAngles.z);
        }
        else
        {
            viewPoint.transform.rotation = Quaternion.Euler(-verticalRotationStore, viewPoint.transform.rotation.eulerAngles.y, viewPoint.transform.rotation.eulerAngles.z);
        }

        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.LeftShift)) { activeMoveSpeed = runSpeed; }
        else { activeMoveSpeed = moveSpeed; }

        float yVel = movement.y;
        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized * activeMoveSpeed;
        movement.y = yVel;
        if (CharCom.isGrounded)
        {
            movement.y = 0; ;
        }

        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.25f, groundLayer);
        if (Input.GetButtonDown("Junp") && isGrounded)
        {
            movement.y = jumpForce;
        }

        movement.y = Physics.gravity.y * Time.deltaTime * gravityMod;
        CharCom.Move(movement * Time.deltaTime);
    }

    private void LateUpdate()
    {
        cam.transform.position = viewPoint.transform.position;
        cam.transform.rotation = viewPoint.transform.rotation;
    }
}