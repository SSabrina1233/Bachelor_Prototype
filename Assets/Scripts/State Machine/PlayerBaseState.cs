
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerBaseState 
{
    // Abstract State

    
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void OnSwitchState(PlayerStateManager player);

    public abstract int ReturnState(PlayerStateManager player);

    //public abstract GameObject OnTriggerEnter(PlayerStateManager player,Collider other);
    //public abstract void OnTriggerExit(PlayerStateManager player,Collider other);


}
