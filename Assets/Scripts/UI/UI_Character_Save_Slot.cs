using System;
using TMPro;
using UnityEngine;

public class UI_Character_Save_Slot : MonoBehaviour
{
    private SaveFileDataWriter saveFileWriter;
    
    [Header("Save Slot")]
    public CharacterSlot characterSlot;
    
    [Header("Character Info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;

    private void OnEnable()
    {
        LoadSaveSlots();
    }

    private void LoadSaveSlots()
    {
        saveFileWriter = new SaveFileDataWriter();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;
        
        // save slot 01
        switch (characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
            
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                
                break;
            case CharacterSlot.CharacterSlot_02:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot_03:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot_04:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot_05:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot_06:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot_07:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            
            case CharacterSlot.CharacterSlot_08:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot_09:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                break;
            case CharacterSlot.CharacterSlot_10:
                saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                // if file exist, get information from it
                if (saveFileWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
                }
                // if it does not, disable this game object
                else
                {
                    gameObject.SetActive(false);
                }
                break;
                
        }

        
        
    }

    public void LoadGameFromChracterSlot()
    {
        WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
        WorldSaveGameManager.instance.LoadGame();
    }

    public void SelectCurrentCharacterSlot()
    {
        TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
    }
}
