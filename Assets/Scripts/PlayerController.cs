
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float throwHeight = 2.0f;

    private PlayerStateManager stateManager;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;
    private bool yButtonPressed = false;

    public static bool CanMove = true;

    private bool thrown = false;
    


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumped = context.action.triggered;
    }

    public void OnCoopMovement(InputAction.CallbackContext context)
    {
        yButtonPressed = context.action.triggered;
    }
    
    public void OnThrow(InputAction.CallbackContext context)
    {
        thrown = context.action.triggered;
    }

    void Update()
    {
        if (CanMove)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }

// Changes the height position of the player..
            if (jumped && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

        }
        
        /*if (merged && thrown)
        {
            playerVelocity.y += Mathf.Sqrt(throwHeight * -3.0f * gravityValue);
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
            CanMove = true;
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.transform.parent = null;
            merged = false;
        }*/
        
    }
    
}
