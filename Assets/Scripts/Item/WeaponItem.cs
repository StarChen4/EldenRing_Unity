using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Scriptable Objects/WeaponItem")]
public class WeaponItem : Item
{
    // animator controller override (change attack animations based on weapon you are using)
    
    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")] 
    public int strengthREQ = 0;
    public int dexREQ = 0;
    public int intREQ = 0;
    public int faithREQ = 0;
    
    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int holyDamage = 0;
    public int lightningDamage = 0;
    
    // weapon guard absorptions (blocking power)
    
    [Header("Weapon Poise")]
    public float poisonDamage = 0;
    // offensive poise bonus when attacking
    
    
    // weapon modifiers
    // light attack
    // heavy attack
    // critical damage
    
    [Header("Stamina Costs")]
    public int baseStaminaCost = 20;
    // running attack stamina cost modifier
    // light attack
    // heavy attack
    
    // item based actions (RB, RT, LB, LT)
    [Header("Actions")] 
    public WeaponItemAction oh_RB_Action; // one-handed right bumper actions

    // ash of war

    // blocking sounds
}
