using com.ols.merge2.Game.Controllers;
using com.ols.merge2.Game.Models;
using OLS_HyperCasual;
using UnityEngine;

public class PreLevelMergeController : MergeMonoController<PreLevelWeaponView, PreLevelWeaponModel>
{
    private PreLevelWeaponController weaponController;
    private PrefabsController prefabsController;

    public override void PostInit()
    {
        weaponController = BaseEntryPoint.Get<PreLevelWeaponController>();
        prefabsController = BaseEntryPoint.Get<PrefabsController>();
    }
    
    public override PreLevelWeaponModel GetMergeResult(Merge2MonoModel<PreLevelWeaponView, PreLevelWeaponModel> model1, Merge2MonoModel<PreLevelWeaponView, PreLevelWeaponModel> model2)
    {
        var mergeModel1 = model1 as PreLevelWeaponModel;
        var mergeModel2 = model2 as PreLevelWeaponModel;
        
        mergeModel1.GetCachedCell().ClearCurrentWeapon();
        
        var cell = mergeModel2.GetCachedCell();
        var weaponView = InstantiateWeaponGameObject(mergeModel2.MergeType);
        var newModel = CreateModel(weaponView, cell);

        newModel.MergeType = mergeModel2.MergeType + 1;
        
        weaponController.RemoveModel(mergeModel1);
        weaponController.RemoveModel(mergeModel2);

        return newModel;
    }
    
    private PreLevelWeaponModel CreateModel(PreLevelWeaponView view, PreLevelCellModel cell)
    {
        var preLevelModel = new PreLevelWeaponModel(view, GetMergeResult);
        preLevelModel.SetCellModel(cell);
        return preLevelModel;
    }

    private PreLevelWeaponView InstantiateWeaponGameObject(EWeaponsType mergeType)
    {
        var gameObject = prefabsController.GetPrefab<PreLevelWeaponView>(mergeType + 1);
        return gameObject;
    }
}