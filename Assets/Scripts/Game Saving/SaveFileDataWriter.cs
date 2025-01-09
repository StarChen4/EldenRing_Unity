using System;
using System.IO;
using UnityEngine;


public class SaveFileDataWriter
{
    public string saveDataDirectoryPath = "";
    public string saveFileName = "";

    public bool CheckToSeeIfFileExists()
    {
        if (File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteSaveFile()
    {
        File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
    }

    public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
    {
        // make a path to save the file
        string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

        try
        {
            // create the directory the file will be written to if it does not exist
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            Debug.Log("Creating save file at path : " + savePath);

            // serialize the C# game data object to json format
            string dataToStore = JsonUtility.ToJson(characterData, true);

            // write the file
            using (FileStream stream = new FileStream(savePath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error whilst tring to save character data, game not saved");
        }
    }
    
    // used to load a save file upon loading a previous game
    public CharacterSaveData LoadSaveFile()
    {
        CharacterSaveData characterData = null;
        
        string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);

        if (File.Exists(loadPath))
        {
            try
            {
                string dataToLoad = "";
                
                using (FileStream stream = new FileStream(loadPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
            
                // deserialize the data from json back to unity C#
                characterData = JsonUtility.FromJson<CharacterSaveData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Can not find the save file");
            }
            
        }
        
        return characterData;
    }
}
