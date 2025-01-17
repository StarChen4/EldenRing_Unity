using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    PlayerManager player;
    
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;
    
    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;
    
    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<PlayerManager>();
        // get our slots
        InitializeWeaponSlot();
    }

    protected override void Start()
    {
        base.Start();
        
        LoadWeaponsOnBothHands();
    }

    private void InitializeWeaponSlot()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponsOnBothHands()
    {
        LoadRightWeapon();
        LoadLeftWeapon();
    }

    #region Right Weapon

    public void SwitchRightWeapon()
    {
        if(!player.IsOwner)
            return;
        
        player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, true, true, true);
        
        // check if the player has another weapon besides the main weapon, if so, never swap to unarmed, rotate between 1 and 2
        // if not, swap to unarmed, then skip the other empty slot and swap back. 

        WeaponItem selectedWeapon = null;
        
        // Disable two-handing if we are two handing
        // check out weapon index
        
        // add one to index to switch to the next weapon
        player.playerInventoryManager.rightHandWeaponIndex += 1;

        if (player.playerInventoryManager.rightHandWeaponIndex < 0 ||
            player.playerInventoryManager.rightHandWeaponIndex > 2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;
            
            // check if we are holding more than one weapon
            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID !=
                    WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    weaponCount++;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unarmedWeapon;
                player.playerNetworkManager.currentRightHandWeaponID.Value
                    = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                
            }

            return;
        }

        foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
        {
            // check to see if this is not the "unarmed" weapon
            if (player.playerInventoryManager
                    .weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID !=
                WorldItemDatabase.instance.unarmedWeapon.itemID)
            {
                selectedWeapon =
                    player.playerInventoryManager.weaponsInRightHandSlots[
                        player.playerInventoryManager.rightHandWeaponIndex];
                // assign the network weapon ID so it switches for all connected clients
                player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager
                    .weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            SwitchRightWeapon();
        }
 
    }
    
    public void LoadRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon != null)
        {
            // remove the old weapon
            rightHandSlot.UnloadWeapon();
            
            // bring in the new weapon
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeapon(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            // assign weapons damage, to its collider
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            
        }
    }


    #endregion

    #region Left Weapon

    public void SwitchLeftWeapon()
    {
        
    }
    
    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            // remove the old weapon
            leftHandSlot.UnloadWeapon();
            
            // bring in the new weapon
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeapon(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            // assign weapons damage, to its collider
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
    }
    

    #endregion
}
