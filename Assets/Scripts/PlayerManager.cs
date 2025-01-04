using UnityEngine;

public class PlayerManager : CharacterManager
{
    PlayerLocomotionManager playerLocomotionManager;
    protected override void Awake()
    {
        base.Awake();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
    }

    protected override void Update()
    {
        base.Update();
        
        // if we don't own this game object, we do not control or edit it.
        if (!IsOwner)
        {
            return;
        }
        // handle all characters movement
        playerLocomotionManager.HandleAllMovement();
    }
}


