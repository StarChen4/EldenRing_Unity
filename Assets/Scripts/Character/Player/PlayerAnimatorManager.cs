using System;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    private PlayerManager player;

    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<PlayerManager>();
    }

    // apply root motion on the player, can be edited
    private void OnAnimatorMove()
    {
        if (player.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }
}
