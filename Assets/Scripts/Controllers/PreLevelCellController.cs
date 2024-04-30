using System;
using System.Collections.Generic;
using System.Linq;
using OLS_HyperCasual;
using UnityEngine;

public class PreLevelCellController : BaseMonoController<PreLevelCellView, PreLevelCellModel>
{
    public Action OnAllCellsInited;
    private int preInitCellCount;
    
    public override PreLevelCellModel AddView(PreLevelCellView view)
    {
        var model = new PreLevelCellModel(view);
        modelsList.Add(model);
        if (modelsList.Count == preInitCellCount)
        {
            OnAllCellsInited?.Invoke();   
        }
        return model;
    }

    public PreLevelCellModel GetEmptyCell()
    {
        foreach (var model in modelsList)
        {
            if (model.HasWeaponModel() == false)
            {
                return model;
            }
        }

        return null;
    }

    public bool HasEmptyCell()
    {
        foreach (var model in modelsList)
        {
            if (model.HasWeaponModel() == false)
            {
                return true;
            }
        }

        return false;
    }

    public PreLevelCellModel GetCellByGameObject(GameObject gameObject)
    {
        foreach (var model in modelsList)
        {
            if (gameObject == model.View.gameObject)
            {
                return model;
            }
        }

        return null;
    }

    public PreLevelCellModel GetCellByIndex(int index)
    {
        Debug.Log("Count Cell in CellController: " + modelsList.Count);
        foreach (var model in modelsList)
        {
            if (model.CellIndex == index)
            {
                return model;
            }
        }

        return null;
    }

    public void ClearData()
    {
        foreach (var model in modelsList)
        {
            //model.ClearData();
            model.View.gameObject.SetActive(false);
        }
        
        //modelsList.Clear();
    }

    public void SetCountCell(int listViewCount)
    {
        preInitCellCount = listViewCount;
    }
}