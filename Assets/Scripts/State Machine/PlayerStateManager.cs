using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))]
public class PlayerStateManager : MonoBehaviour
{
    // PlayerController
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float magnetismStrength = 1.0f;

    public Rigidbody rb;
    private Vector3 PlayerMovementInput;
    public float sensitivity, maxForce;
    private float lookRotation;

    //private Vector3 playerVelocity;
    private bool groundedPlayer;

    private Vector2 movementInput = Vector2.zero;
    public static bool CanMove = true;

    //--------------------------------------------------------
    // Context of FSM
    private PlayerBaseState currentState;
    private PlayerBaseState secondPlayerCurrentState;
    public PlayerPositiveState PositiveState = new PlayerPositiveState();
    public PlayerNegativeState NegativeState = new PlayerNegativeState();

    // Collision Detection
    private GameObject secondPlayer = null;
    private bool inRange = false;
    private bool merged = false;
    private Vector3 mergeOffset = new Vector3(0,1,0);
    private bool xButtonPressed;
    
    void Start()
    {
        
        currentState = NegativeState;
        secondPlayerCurrentState = null;
        
        currentState.EnterState(this);
    }
   
    //Events for InputSystem
    public void OnMove(InputAction.CallbackContext context)
    {

        //movementInput = context.ReadValue<Vector2>();
        movementInput = context.ReadValue<Vector2>();
    }

    /*public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }*/

    public void OnJump(InputAction.CallbackContext context)
    {
        //Jump();
    }

    public void OnMagnetAbility(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            if (!context.performed)
            {
                return;
            }
            currentState.OnSwitchState(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
       currentState.UpdateState(this);

       secondPlayerCurrentState = secondPlayer.gameObject.GetComponent<PlayerStateManager>().currentState;

       PlayerMovementInput = new Vector3(movementInput.x, 0f, movementInput.y);

        Move();

        if (inRange)
        {
            if (currentState.ReturnState(this) == secondPlayerCurrentState.ReturnState(this))
            {
                repel();
            }
            if (currentState.ReturnState(this) != secondPlayerCurrentState.ReturnState(this))
            {
                attract();
            }
        }

    }

    private void FixedUpdate()
    {
       /* if (CanMove)
        {
            Move();
        }*/

    }

    void Move()
    {
        Vector3 MoveVector = transform.TransformDirection(PlayerMovementInput) * playerSpeed;
        rb.velocity = new Vector3(MoveVector.x, rb.velocity.y, MoveVector.z);


       /* Vector3 currentVelocity = rb.velocity;
        Vector3 targetVelocity = new Vector3(movementInput.x, 0, movementInput.y);
        targetVelocity *= playerSpeed;
      

        //Align direction
        targetVelocity = transform.TransformDirection(targetVelocity);

        //Calculate forces
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        //velocityChange = new Vector3(velocityChange.x, 0, velocityChange.y);

        //Limit force
        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);*/


        // rotate towards movement
        //transform.Rotate(new Vector3(0, Vector2.Angle(Vector2.zero, movementInput), 0));

        //transform.rotation = Quaternion.LookRotation(movementInput);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementInput), Time.deltaTime * 40f);


    }

    void Jump()
    {
        //playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        Vector3 jumpForces = Vector3.zero;

        if (groundedPlayer)
        {
            jumpForces = Vector3.up * jumpHeight;
        }

        rb.AddForce(jumpForces, ForceMode.VelocityChange);
    }

    Vector3 targetDirection(Vector3 target)
    {
        return target - this.transform.position;
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
        Debug.Log("SwitchedState...");
    }

    public void repel()
    {
        //Rigidbody tempRb = this.GetComponent<Rigidbody>();
        
        //tempRb.AddForce(targetDirection(this.transform.position).normalized * magnetismStrength* currentState.ReturnState(this), ForceMode.Force);
        
        Debug.Log(secondPlayer.gameObject.GetComponent<PlayerStateManager>().currentState + "Push away" + this.currentState);
    }

    public void attract()
    {
        Debug.Log(secondPlayer.gameObject.GetComponent<PlayerStateManager>().currentState + "attract" + this.currentState);
        Debug.Log(secondPlayer.gameObject.GetComponent<PlayerDetails>().playerID + " is the other player, " + this.gameObject.GetComponent<PlayerDetails>().playerID + " is me ");
    }
    
    /*private void Detach()
    {
        PlayerController.CanMove = true;
        gameObject.GetComponent<Collider>().enabled = true;
        gameObject.transform.parent = null;
        merged = false;
    }*/

    public void SetGrounded(bool state)
    {
        groundedPlayer = state;
    }

    private void Attach()
    {
        Debug.Log("merged...");

        PlayerController.CanMove = false;
        transform.position = secondPlayer.transform.position + mergeOffset ;
        gameObject.transform.parent = secondPlayer.transform;
            
        gameObject.GetComponent<Collider>().enabled = false;
        merged = true;
    }

    public GameObject OnTriggerEnter(Collider other)
    {
    
        secondPlayer = other.gameObject;
       
        if (other.gameObject.GetComponent<PlayerDetails>().playerID != this.GetComponent<PlayerDetails>().playerID)
        {
            inRange = true;
            Debug.Log("in Range");
            secondPlayer = other.gameObject;
            

            //return mergeID = other.gameObject.GetComponent<PlayerDetails>().playerID;

        }

        return secondPlayer;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerDetails>().playerID != this.GetComponent<PlayerDetails>().playerID)
        {
            inRange = false;
            Debug.Log("Out of Range");
        }
    }
    
    

}
