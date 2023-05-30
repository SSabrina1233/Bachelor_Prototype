
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerBackup : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float throwHeight = 2.0f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 movementInput = Vector2.zero;
    private bool jumped = false;
    private bool yButtonPressed = false;
    private bool inRange = false;
    private bool merged = false;
    
    public bool CanMove = true;
    private GameObject transformer;
    private Vector3 mergeOffset = new Vector3(0,1,0);

    private float timer = 0.0f;


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
        
        if (merged && thrown)
        {
            playerVelocity.y += Mathf.Sqrt(throwHeight * -3.0f * gravityValue);
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
            CanMove = true;
            gameObject.GetComponent<Collider>().enabled = true;
            gameObject.transform.parent = null;
            merged = false;
        }
        
        if (yButtonPressed && timer >= 5.0f)
        {
            if (!merged && inRange)
            {
                Attach();
            }
            else
            {
                Detach();
            }

            timer = 0.0f;

        }

        timer++;
    }

    private void Detach()
    {
        CanMove = true;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.transform.parent = null;
        merged = false;
    }

    private void Attach()
    {
        Debug.Log("merged...");

        CanMove = false;
        transform.position = transformer.transform.position + mergeOffset ;
        gameObject.transform.parent = transformer.transform;
            
        gameObject.GetComponent<Collider>().enabled = false;
        merged = true;
    }

    private GameObject OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.GetComponent<PlayerDetails>().playerID != this.GetComponent<PlayerDetails>().playerID)
        {
            inRange = true;
            Debug.Log("in Range");
            transformer = other.gameObject;

            //return mergeID = other.gameObject.GetComponent<PlayerDetails>().playerID;

        }

        return transformer;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerDetails>().playerID != this.GetComponent<PlayerDetails>().playerID)
        {
            inRange = false;
            Debug.Log("Out of Range");
        }
    }
}
