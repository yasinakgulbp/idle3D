using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private JoystickController joystickController;
    private CharacterController characterController;
    [SerializeField] private PlayerAnimator playerAnimator; 
    Vector3 moveVector;

    [Header("Settings")]
    [SerializeField] private int moveSpeed;

    private float gravity = -9.81f;
    private float gravityMultiplier = 3f;
    private float gravityVelocity;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveVector = joystickController.GetMovePosition() * moveSpeed * Time.deltaTime / Screen.width;

        moveVector.z = moveVector.y;
        moveVector.y = 0;
        
        playerAnimator.ManageAnimations(moveVector);

        ApplyGraviy();
        characterController.Move(moveVector);
    }

    private void ApplyGraviy()
    {
        if (characterController.isGrounded && gravityVelocity < 0.0f) 
        {
            gravityVelocity = -1f;
        }
        else
        {
            gravityVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }

        moveVector.y += gravityVelocity;
    }
}
