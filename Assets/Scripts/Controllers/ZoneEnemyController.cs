using System;
using OLS_HyperCasual;
using UnityEngine;

public class ZoneEnemyController : BaseMonoController<ZoneEnemyView, ZoneEnemyModel>
{
    public Action OnAllZonesDestroyed;
    
    public override ZoneEnemyModel AddView(ZoneEnemyView view)
    {
        var model = new ZoneEnemyModel(view, DeleteModelAction);
        modelsList.Add(model);
        
        return model;
    }

    public bool IsAnyZoneModelContainsObject(Vector3 position)
    {
        foreach (var model in modelsList)
        {
            if (model.IsContains(position))
            {
                return true;
            }
        }

        return false;
    }

    public ZoneEnemyModel GetModelByObject(ZoneEnemyView zoneView)
    {
        foreach (var model in modelsList)
        {
            if (model.View.gameObject == zoneView.gameObject)
            {
                return model;
            }
        }

        return null;
    }

    private void DeleteModelAction(ZoneEnemyModel zoneModel)
    {
        modelsList.Remove(zoneModel);

        if (modelsList.Count == 0)
        {
            OnAllZonesDestroyed?.Invoke();
            OnAllZonesDestroyed = null;
        }
    }
}