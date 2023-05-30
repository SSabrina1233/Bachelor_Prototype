using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPositiveState : PlayerBaseState
{
    private GameObject secondPlayer = null;
    
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered positively charged state");
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }

    public override void OnSwitchState(PlayerStateManager player)
    {
        Debug.Log("Switching to negatively charged");
        player.SwitchState(player.NegativeState);
    }
    
    public override int ReturnState(PlayerStateManager player)
    {
        return 1;
    }
}
