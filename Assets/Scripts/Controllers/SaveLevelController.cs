using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using OLS_HyperCasual;
using UnityEngine;

public class SaveLevelController : BaseController
{
    //private List<LevelDataModel> levelDataModels;
    private BinaryFormatter binaryFormatter;
    private string path;

    public SaveLevelController()
    {
        //levelDataModels = new List<LevelDataModel>();
        binaryFormatter = new BinaryFormatter();
        path = Application.persistentDataPath + "/gameSave.ss";
        Debug.Log(path);
    }

    public void SaveLevelData(int levelIndex, List<PreLevelWeaponModel> weaponModels, List<int> deathWeaponsIndex)
    {
        LevelDataModel levelData = new LevelDataModel(levelIndex, weaponModels, deathWeaponsIndex);

        var models = DeserializeData();

        if (models == null)
        {
            models = new List<LevelDataModel>();
        }

        if (IsContains(models, levelData.LevelIndex, out var model))
        {
            models.Remove(model);
        }

        models.Add(levelData);

        SerializeData(models);
    }

    public LevelDataModel GetLevelData(int levelIndex)
    {
        var models = DeserializeData();

        if (models != null)
        {
            if (IsContains(models, levelIndex, out var dataModel))
            {
                return dataModel;
            }
        }

        return null;
    }

    private void SerializeData(List<LevelDataModel> levelDataModels)
    {
        FileStream stream = new FileStream(path, FileMode.Create);

        binaryFormatter.Serialize(stream, levelDataModels);
        Debug.Log("Serialize");
        stream.Close();
    }

    public List<LevelDataModel> DeserializeData()
    {
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            var listData = (List<LevelDataModel>) binaryFormatter.Deserialize(stream);
            stream.Close();
            return listData;
        }

        return null;
    }

    private bool IsContains(List<LevelDataModel> levelDataModels, int index, out LevelDataModel indexModel)
    {
        foreach (var levelDataModel in levelDataModels)
        {
            if (levelDataModel.LevelIndex == index)
            {
                indexModel = levelDataModel;
                return true;
            }
        }

        indexModel = null;
        return false;
    }

    private void RemoveModel(LevelDataModel model)
    {
        //levelDataModels.Remove(model);
    }
}