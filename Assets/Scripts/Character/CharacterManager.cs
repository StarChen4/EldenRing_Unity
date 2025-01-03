using System;
using Unity.Netcode;
using UnityEngine;

public class CharacterManager : NetworkBehaviour
{
    public CharacterController characterController;
    
    CharacterNetworkManager characterNetworkManager;
    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        characterController = GetComponent<CharacterController>();
        characterNetworkManager = GetComponent<CharacterNetworkManager>();
    }

    protected virtual void Update()
    {
        // if this character is being controlled from our side, then assign its network position of our transform
        if (IsOwner)
        {
            characterNetworkManager.networkPosition.Value = transform.position;
            characterNetworkManager.networkRotation.Value = transform.rotation;
        }
        // if this character is being controlled from elsewhere, then assign its position here locally by the position of its network transform
        else
        {
            // position
            transform.position = Vector3.SmoothDamp(
                transform.position, 
                characterNetworkManager.networkPosition.Value,
                ref characterNetworkManager.networkPositionVelocity, 
                characterNetworkManager.networkPositionSmoothTime);
            
            // rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                characterNetworkManager.networkRotation.Value,
                characterNetworkManager.networkRotationSmoothTime);
        }
    }
}
