using System;
using Unity.Netcode;
using UnityEngine;

public class CharacterAnimatorManager : MonoBehaviour
{
    private CharacterManager character;

    int vertical;
    int horizontal;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }
    
    public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
    {

        float horizontalAmount = horizontalMovement;
        float verticalAmount = verticalMovement;
        
        if (isSprinting)
        {
            verticalAmount = 2;
        }
        
        // optional
        character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
        character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);

    }

    public virtual void PlayTargetActionAnimation(
        string targetAnimation, 
        bool isPerformingAction, 
        bool applyRootMotion = true, 
        bool canRotate = false, 
        bool canMove = false)
    {
        
        character.applyRootMotion = applyRootMotion;
        character.animator.CrossFade(targetAnimation, 0.2f);
        
        // can be used to stop character from attempting new actions
        // this flag will stop new actions
        character.isPerformingAction = isPerformingAction;
        character.canRotate = canRotate;
        character.canMove = canMove;
        
        // tell the server/host played an animation and to play it for everyone else
        character.characterNetworkManager.NotifyTheServerOfAnActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion
        );
    }
}
