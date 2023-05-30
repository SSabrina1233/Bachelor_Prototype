using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerNegativeState : PlayerBaseState
{
    private GameObject secondPlayer = null;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("Entered negatively charged state");
        return;
    }

    public override void UpdateState(PlayerStateManager player)
    {
    }

    public override void OnSwitchState(PlayerStateManager player)
    {
        Debug.Log("Switching to positively charged");
        player.SwitchState(player.PositiveState);
    }

    public override int ReturnState(PlayerStateManager player)
    {
        return -1;
    }
}
