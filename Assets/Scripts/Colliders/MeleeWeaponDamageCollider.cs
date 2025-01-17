using UnityEngine;

public class MeleeWeaponDamageCollider : DamageCollider
{
    [Header("Attacking Character")] 
    public CharacterManager characterCausingDamage; // when calculating damage this is used to check for attackers damage modifiers, effects etc.
    
}
