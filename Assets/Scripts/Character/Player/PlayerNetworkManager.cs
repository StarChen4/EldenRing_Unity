using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkManager : CharacterNetworkManager
{
    public NetworkVariable<FixedString64Bytes> characterName = new NetworkVariable<FixedString64Bytes>("character", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    
}
