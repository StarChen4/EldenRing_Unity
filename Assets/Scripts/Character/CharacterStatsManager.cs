using System;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;
    
    [Header("Stamina Renegeration")] 
    [SerializeField] private float staminaRegenerationAmount = 2;
    private float staminaRegenerationTimer = 0;
    private float staminaTickTimer = 0;
    [SerializeField] private float staminaRegenerationDelay = 2;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }

    protected virtual void Start()
    {
        
    }

    public int CalculateHealthBasedOnVitalityLevel(int vitality)
    {
        float health = 0;
        
        // create an equation
        health = vitality * 15;

        return Mathf.RoundToInt(health);
    }
    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;
        
        // create an equation
        stamina = endurance * 10;

        return Mathf.RoundToInt(stamina);
    }
    
    public virtual void RegenerateStamina()
    {
        if(!character.IsOwner)
            return;
        
        if(character.characterNetworkManager.isSprinting.Value)
            return;
        
        if(character.isPerformingAction)
            return;
        
        staminaRegenerationTimer += Time.deltaTime;
        
        // only regenerate stamina after a delay
        if (staminaRegenerationTimer >= staminaRegenerationDelay)
        {
            // when stamina is not at max value
            if (character.characterNetworkManager.currentStamina.Value < character.characterNetworkManager.maxStamina.Value)
            {
                staminaTickTimer += Time.deltaTime;
                
                // regenerate at a specific value
                if (staminaTickTimer >= 0.1)
                {
                    staminaTickTimer = 0;
                    character.characterNetworkManager.currentStamina.Value += staminaRegenerationAmount;
                }
            }
        }
    }

    public virtual void ResetStaminaRegenTimer(float previousStaminaAmount, float currentStaminaAmount)
    {
        // we only want to reset the timer if the action used stamina
        if (currentStaminaAmount < previousStaminaAmount)
        {
            staminaRegenerationTimer = 0;
        }
    }
}
