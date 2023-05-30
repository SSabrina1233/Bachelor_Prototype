using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public PlayerStateManager playerController;
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == playerController.gameObject)
        {
            return;
        }

        playerController.SetGrounded(true);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }

        playerController.SetGrounded(false);
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerController.gameObject)
        {
            return;
        }
        playerController.SetGrounded(true);
    }
}
