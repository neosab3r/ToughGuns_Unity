using System;
using System.Collections.Generic;
using OLS_HyperCasual;
using UnityEngine;
using Object = UnityEngine.Object;

public class PreLevelWeaponController : BaseMonoController<PreLevelWeaponView, PreLevelWeaponModel>
{
    public override bool HasUpdate => true;

    private const int LayerWeapon = 6;
    private const int LayerCell = 3;

    private PreLevelMergeController mergeController;
    private PreLevelCellController cellController;
    private PrefabsController prefabsController;

    private PreLevelCellModel currentCellModel;
    private PreLevelWeaponModel currentWeaponModel;

    public PreLevelWeaponController()
    {
        mergeController = BaseEntryPoint.Get<PreLevelMergeController>();
        cellController = BaseEntryPoint.Get<PreLevelCellController>();
        prefabsController = BaseEntryPoint.Get<PrefabsController>();
    }

    public override PreLevelWeaponModel AddView(PreLevelWeaponView view)
    {
        var model = new PreLevelWeaponModel(view, mergeController.GetMergeResult);

        modelsList.Add(model);

        return model;
    }

    public void RemoveModel(PreLevelWeaponModel model)
    {
        model.ClearData();
        modelsList.Remove(model);
    }

    public List<PreLevelWeaponModel> GetListModels()
    {
        return modelsList;
    }

    public void CreateWeapon(EWeaponsType weaponType)
    {
        var weaponGO = prefabsController.GetPrefab<PreLevelWeaponView>(weaponType);
        
        var model = AddView(weaponGO);
        
        model.SetCellModel(cellController.GetEmptyCell());
        
        model.MergeType = weaponType;
    }

    public void CreateWeaponInData(LevelDataModel levelDataModel)
    {
        if (levelDataModel == null)
        {
            return;
        }

        foreach (var weapon in levelDataModel.WeaponDatas)
        {
            var weaponGO = prefabsController.GetPrefab<PreLevelWeaponView>(weapon.WeaponType);

            var model = AddView(weaponGO);

            var cell = cellController.GetCellByIndex(weapon.CellIndex);
            if (cell == null)
            {
                Debug.Log("Cell null -- Index = " + weapon.CellIndex);
            }
            
            model.SetCellModel(cell);

            model.MergeType = weapon.WeaponType;
        }
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

    private PreLevelWeaponModel GetModelByGameObject(GameObject gameObject)
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

    private GameObject GetGameObjectByScreenPosition(Vector2 screenPos, int layerMask)
    {
        var ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit, 1000, 1 << layerMask) == false)
        {
            return null;
        }

        return hit.transform.gameObject;
    }
    
    private void MergeWeapon(PreLevelCellModel cellModel)
    {
        var model = currentWeaponModel.Merge(cellModel.GetWeaponModel());
        modelsList.Add(model);
        currentWeaponModel = model;
    }

    private void DragCachedWeaponModel()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = -0.5f;
        mouseWorldPosition.y += -1.1f;
        currentWeaponModel.OnDrag(mouseWorldPosition);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Update(float dt)
    {
        if (Input.GetMouseButtonDown(0))
        {
            var result = GetGameObjectByScreenPosition(Input.mousePosition, LayerWeapon);
            currentWeaponModel = GetModelByGameObject(result);
            currentWeaponModel?.OnBeginDrag(Input.mousePosition);
        }

        if (currentWeaponModel == null)
        {
            return;
        }

        DragCachedWeaponModel();

        if (Input.GetMouseButtonUp(0))
        {
            var cellResult = GetGameObjectByScreenPosition(Input.mousePosition, LayerCell);
            
            var cellModel = cellController.GetCellByGameObject(cellResult);
            
            if (cellModel != null && cellModel.HasWeaponModel())
            {
                if (cellModel.GetWeaponModel() != currentWeaponModel && cellModel.CompareMergeType(currentWeaponModel.MergeType))
                {
                    MergeWeapon(cellModel);
                }
            }
            
            currentWeaponModel.EndDrag(cellModel);
            currentWeaponModel = null;
        }
    }
}