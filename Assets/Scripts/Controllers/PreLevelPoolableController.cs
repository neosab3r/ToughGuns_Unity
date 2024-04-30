/*using System;
using System.Collections.Generic;
using OLS_HyperCasual;
using UnityEngine;

public class PreLevelPoolableController : PoolableController<PreLevelWeaponModel, PreLevelWeaponView>
{
    private List<PreLevelWeaponModel> modelList = new();

    public PreLevelPoolableController()
    {
        var countWeapon = Enum.GetNames(typeof(EMergeWeaponType)).Length;
        var resourcesController = BaseEntryPoint.Get<ResourcesController>();
        var soGameSettings =
            resourcesController.GetResource<GameSettingsSO>(GameResourceConstants.GameSettingsSO, false);

        for (var i = 0; i < countWeapon; i++)
        {
            var nameWeapon = GetTypeName((EMergeWeaponType) i);
            var path = GetResourcePath(nameWeapon, soGameSettings);
            var prefab = resourcesController.GetResource<PreLevelWeaponView>(path, false);
            PreInitPool(nameWeapon, prefab);
        }
    }

    public PreLevelWeaponModel ShowTeammate(string teammateType)
    {
        var newModel = true;
        var weapon = GetFromPool(teammateType);
        //teammate.ShowTeammate(true);

        foreach (var model in modelList)
        {
            if (model == weapon)
            {
                newModel = false;
            }
        }

        if (newModel)
        {
            modelList.Add(weapon);
        }

        return weapon;
    }

    public void HideTeammate(PreLevelWeaponModel model)
    {
        ReturnToPool(model);
        //model.ShowTeammate(false);
    }

    /*public PreLevelWeaponModel GetSelectedTeammate(RaycastHit hit)
    {
        foreach (var model in modelList)
        {
            if (model.CachedTransform == hit.transform)
            {
                return model;
            }
        }

        return null;
    }#1#

    private string GetTypeName(Enum type)
    {
        return type.ToString();
    }

    private string GetResourcePath(string nameWeapon, GameSettingsSO soGameSettings)
    {
        var name = soGameSettings.GetValue(nameWeapon);
        return $"Prefabs/Weapons/{name}";
    }
}*/