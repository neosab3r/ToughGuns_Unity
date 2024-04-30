using System;
using OLS_HyperCasual;
using UnityEngine;
using Object = UnityEngine.Object;

public class PrefabsController : BaseController
{
    private ResourcesController resourcesController;
    public PrefabsController()
    {
        resourcesController = BaseEntryPoint.Get<ResourcesController>();
    }

    public T GetPrefab<T>(Enum enumType, Enum suffixType = null, bool isInstantiate = true) where T : MonoBehaviour
    {
        var soGameSettings = resourcesController.GetResource<GameSettingsSO>(GameResourceConstants.GameSettingsSO, false);
        
        var enumString = enumType.ToString();
        var suffixEnumString = "";

        if (suffixType != null)
        {
            suffixEnumString = suffixType.ToString();
        }
        
        var path = GetResourcePath(enumString, suffixEnumString, soGameSettings);
        var prefab = resourcesController.GetResource<T>(path, false);
        if (isInstantiate == true)
        {
            var gameObject = Object.Instantiate(prefab);
            return gameObject;
        }

        return prefab;
    }

    private string GetResourcePath(string key, string suffix, GameSettingsSO soGameSettings)
    {
        var value = soGameSettings.GetValue(String.Concat(key, suffix));
        return $"Prefabs/{value}";
    }
}