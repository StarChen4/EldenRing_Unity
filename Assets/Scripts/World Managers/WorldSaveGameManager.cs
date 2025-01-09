using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSaveGameManager : MonoBehaviour
{
    public static WorldSaveGameManager instance;

    public PlayerManager player;

    [Header("Save/Load")] 
    [SerializeField] private bool saveGame;
    [SerializeField] private bool loadGame;
    
    [Header("World Scene Index")]
    [SerializeField] int worldSceneIndex = 1;
    
    [Header("Save Data Wrirter")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")] 
    public CharacterSlot currentCharacterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;
    private string saveFileName;
    
    
    [Header("Character Slots")] 
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;
    public CharacterSaveData characterSlot10;
    
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
        LoadAllCharacterProfiles();
    }

    private void Update()
    {
        if (saveGame)
        {
            saveGame = false;
            SaveGame();
        }

        if (loadGame)
        {
            loadGame = false;
            LoadGame();
        }
    }

    public string DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "CharacterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "CharacterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "CharacterSlot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "CharacterSlot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "CharacterSlot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                fileName = "CharacterSlot_10";
                break;
        }
        
        return fileName;
    }

    public void AttempToCreateNewGame()
    {   
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        
        // check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // if this profile slot is not taken, make a new one using this slot
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }
        
        // check to see if we can create a new save file (check for other existing files first)
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            // if this profile slot is not taken, make a new one using this slot
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();
            StartCoroutine(LoadWorldScene());
            return;
        }
        
        // if there are no free slots, notify the player
        TitleScreenManager.Instance.DisplayNoFreeCharacterSlotsPopUp();
    }

    public void LoadGame()
    {
        // load a previous file, with a file name depending on which slot we are using
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
        
        saveFileDataWriter = new SaveFileDataWriter();
        // generally works on multiple machine types (Application.persistentDataPath)
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());
    }

    public void SaveGame()
    {
        // save the current file under a file name depending on which slot we are using
        saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
        
        saveFileDataWriter = new SaveFileDataWriter();
        // generally works on multiple machine types (Application.persistentDataPath)
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        
        // pass the player info, from game, to their save file
        player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
        
        // write that info onto a json file, saved to this machine
        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }
    
    // load all character profiles on device when starting game
    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName =
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        characterSlot05 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
        characterSlot06 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
        characterSlot07 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
        characterSlot08 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
        characterSlot09 = saveFileDataWriter.LoadSaveFile();
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
        characterSlot10 = saveFileDataWriter.LoadSaveFile();
        
    }
    
    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        
        player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
        
        yield return null;
    }

    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }
}
