using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance {get; private set;}
    
    [Header("Network Join")] [SerializeField]
    private bool startGameAsClient;

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
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (startGameAsClient)
        {
            startGameAsClient = false;
            // must first shut down because we have started as a host during the title screen
            NetworkManager.Singleton.Shutdown();
            // we then restart, as a client
            NetworkManager.Singleton.StartClient();
        }
    }
}
