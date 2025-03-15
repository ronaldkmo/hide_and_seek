using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovementv2 : MonoBehaviour
{
    CharacterController characterController;
    Transform playerContainer, cameraContainer;
    private Camera playerCamera;
    public float speed = 2f;
    public float jumpSpeed = 2f;
    public float mouseSensitivity = 2f;
    public float gravity = 20.0f;
    public float lookUpClamp = -30f;
    public float lookDownClamp = 60f;
    private Vector3 moveDirection = Vector3.zero;
    float rotateX, rotateY;
    private bool isJumping = false;

    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
    }    
    void Update(){
        Movement();
        RotateAndLook();
    }

    void Movement() {
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        inputDirection = transform.TransformDirection(inputDirection);
        
        // When grounded, set y-axis to zero (to ignore it)
        if (characterController.isGrounded)
        {
            isJumping = false;
            
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
         }
        
        else{
            moveDirection.x = Mathf.Lerp(moveDirection.x, inputDirection.x * speed, 0.001f);
            moveDirection.z = Mathf.Lerp(moveDirection.z, inputDirection.z * speed, 0.001f);
        }

        if (Input.GetButton("Jump"))
        {
            if (!isJumping) {
                moveDirection.y = jumpSpeed;
                isJumping = true;
            }
        }   
        
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void RotateAndLook() {
        rotateX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotateY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotateY = Mathf.Clamp(rotateY, lookUpClamp, lookDownClamp);
        transform.Rotate(0f, rotateX, 0f);
        
        playerCamera.transform.localRotation = Quaternion.Euler(rotateY, 0f, 0f);

    }
}