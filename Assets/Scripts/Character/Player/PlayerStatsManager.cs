using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    private PlayerManager player;
    
    protected override void Awake()
    {
        base.Awake();
        
        player = GetComponent<PlayerManager>();
    }

    protected override void Start()
    {
        base.Start();
        
        // when we make a new character , set the stats depending on the class, this will be calculated there
        // until then however, stats are never calculated, so we do it here, is a save file exist, it will be over witten when loading to a scene
        CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
        CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
    }
}
