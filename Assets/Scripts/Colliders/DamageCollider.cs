using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Collider")]
    protected Collider damageCollider;
    
    [Header("Damage")]
    public float physicalDamage = 0; // in future will be split into "standard", "strike", "slash" and "pierce"
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;

    [Header("Contact Point")] 
    private Vector3 contactPoint;
    
    [Header("Character Damaged")]
    protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();
    
    private void OnTriggerEnter(Collider other)
    {
        CharacterManager damageTarget = other.GetComponent<CharacterManager>();  
        
        if (damageTarget != null)
        {
            contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            
            // check if we can damage this target based on friendly fire
            
            // check if target is blocking
            
            // check if target is invulnerable
            
            // damage
            DamageTarget(damageTarget);
        }

    }
    
    protected virtual void DamageTarget(CharacterManager damageTarget)
    {
        // same target won't be damaged more than once
        if (charactersDamaged.Contains(damageTarget))
            return;
        
        charactersDamaged.Add(damageTarget);

        TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
        damageEffect.physicalDamage = physicalDamage;
        damageEffect.magicDamage = magicDamage;
        damageEffect.fireDamage = fireDamage;
        damageEffect.lightningDamage = lightningDamage;
        damageEffect.holyDamage = holyDamage;
        damageEffect.contactPoint = contactPoint;
        
        damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
    }

    public virtual void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public virtual void DisableDamageCollider()
    {
        damageCollider.enabled = false;
        charactersDamaged.Clear(); // reset the characters that have been hit when reset the collider, so they mey be hit again
    }
}
