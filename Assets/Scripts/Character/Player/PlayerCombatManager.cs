using UnityEngine;

public class PlayerCombatManager : CharacterCombatManager
{
    PlayerManager player;
    
    public WeaponItem currentWeaponBeingUsed;

    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<PlayerManager>();
    }

    public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
    {
        // perform the action locally
        weaponAction.AttemptToPerformAction(player, weaponPerformingAction);
        
        // notify the server we have performed the action, so perform on other clients
            
    }
}
