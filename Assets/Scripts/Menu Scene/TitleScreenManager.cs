using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class TitleScreenManager : MonoBehaviour
{
    public static TitleScreenManager Instance;
    
    [Header("Menu")]
    [SerializeField] private GameObject titleScreenMainMenu;
    [SerializeField] private GameObject titleScreenLoadMenu;
    
    [Header("Buttons")] 
    [SerializeField] private Button loadMenuReturnButton;
    [SerializeField] private Button mainMenuNewGameButton;
    [SerializeField] private Button mainMenuLoadGameButton;
    
    [Header("Pop Ups")]
    [SerializeField] GameObject noCharacterSlotsPopUp;
    [SerializeField] Button noCharacterSlotsOkayButton;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartNetworkAsHost()
    {
        NetworkManager.Singleton.StartHost();
        
    }

    public void StartNewGame()
    {
        WorldSaveGameManager.instance.AttempToCreateNewGame();

    }

    public void OpenLoadGameMenu()
    {
        // cloase main menu and open load menu
        titleScreenMainMenu.SetActive(false);
        titleScreenLoadMenu.SetActive(true);
        
        // select the return button first
        loadMenuReturnButton.Select();
    }

    public void CloseLoadGameMenu()
    {
        // cloase load menu and open main menu
        titleScreenLoadMenu.SetActive(false);
        titleScreenMainMenu.SetActive(true);
        
        // select the load game button first
        mainMenuLoadGameButton.Select();
    }

    public void DisplayNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(true);
        noCharacterSlotsOkayButton.Select();
    }

    public void CloseNoFreeCharacterSlotsPopUp()
    {
        noCharacterSlotsPopUp.SetActive(false);
        mainMenuNewGameButton.Select();
    }
}
