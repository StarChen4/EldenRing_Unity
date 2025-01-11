using System;
using System.Collections.Generic;
using UnityEngine;

public class WorlCharacterEffectsManager : MonoBehaviour
{
    public static WorlCharacterEffectsManager instance;

    [SerializeField] private List<InstantCharacterEffect> instantEffects;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        GenerateEffectsIDs();
    }

    private void GenerateEffectsIDs()
    {
        for (int i = 0; i < instantEffects.Count; i++)
        {
            instantEffects[i].instantEffectID = i;
        }
    }
}
