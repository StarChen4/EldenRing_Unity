using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    // process instant effects (take damage, heal)
    
    // process timed effects (poison, build ups)
    
    // process static effects (adding/removing buffs)

    private CharacterManager character;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    
    public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
    {
        // take in an effect
        // process it
        effect.ProcessEffect(character);
    }
}
