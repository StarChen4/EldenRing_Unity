using UnityEngine;

[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
public class TakeDamageEffect : InstantCharacterEffect
{
    [Header("Character Causing Damage")] 
    public CharacterManager characterCausingDamage; // if the damage is caused by another character
    
    [Header("Damage")]
    public float physicalDamage = 0; // in future will be split into "standard", "strike", "slash" and "pierce"
    public float magicDamage = 0;
    public float fireDamage = 0;
    public float lightningDamage = 0;
    public float holyDamage = 0;
    
    [Header("Final Damage")]
    private int finalDamageDealt = 0; // the damage the character takes after all calculation

    [Header("Poise")] 
    public float poiseDamage = 0;
    public bool poiseIsBroken = false; // if a character's poise is broken, they will be stunned and play a damage animation
    
    // TODO:build ups
    // build up effect amounts
    
    [Header("Animation")] 
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("Sound FX")] 
    public bool willPlayDamageSFX = true;
    public AudioClip elementalDamageSoundFX; // used on top of regular SFX if there is elemental damage present

    [Header("Direction Damage Taken From")]
    public float angleHitFrom; // used to determine what damage animation to play
    public Vector3 contactPoint; // used to determine where the blood FX instantiate
    
    
    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);
        
        // dead character do not process damage effect
        if (character.isDead.Value)
            return;
        
        // check for invulnerability
        
        // calculate damage
        CalculateDamage(character);
        
        // check which direction the damage came from
        // play a damage animation
        // check for build up
        // play damage sound FX
        // play visual FX
        
        // if character is AI, check for new target if character causing damage is present
    }

    private void CalculateDamage(CharacterManager character)
    {
        if(!character.IsOwner)
            return;
        
        if (characterCausingDamage != null)
        {
            // check for damage modifiers and modify base damage(physical damage buff, elemental damage buff etc)
        }
        
        // check character for flat defenses and subtract them from the damage
        
        // check for character armor absorptions, and subtract the percentage from the damage
        
        // add all damage types together, and apply final damage
        finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightningDamage + holyDamage);

        if (finalDamageDealt <= 0)
        {
            finalDamageDealt = 1;
        }
        
        Debug.Log("Final Damage Given: " + finalDamageDealt);
        // apply damage to the character
        character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
        
        // calculate poise damage to determine if the character will be stunned
    }
}
