using UnityEngine;

[CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
public class WeaponItemAction : ScriptableObject
{
    public int actionID;

    public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        // keep track of which weapon is performing an attack
        if (playerPerformingAction.IsOwner)
        {
            playerPerformingAction.playerNetworkManager.currentWeaponBeingUsed.Value= weaponPerformingAction.itemID;
        }
        
        Debug.Log("The action is performed");
    }
}
