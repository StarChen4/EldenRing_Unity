using UnityEngine;
[CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Stamina Damage")]
public class TakeStaminaDamageEffect : InstantCharacterEffect
{
    public float staminaDamage;
    public override void ProcessEffect(CharacterManager character)
    {
        base.ProcessEffect(character);
        CalculateStaminaDamage(character);
        
    }

    private void CalculateStaminaDamage(CharacterManager character)
    {
        // compared the base stamina damage against other player effects/ modifiers
        if (character.IsOwner)
        {
            Debug.Log("Character is taking :" + staminaDamage + " stamina damage");
            character.characterNetworkManager.currentStamina.Value -= staminaDamage;
        }
        // change the value before subtracting/ adding it
        
        //
    }
}
