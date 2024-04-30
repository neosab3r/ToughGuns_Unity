using System;
using OLS_HyperCasual;
using UnityEngine;
using Object = UnityEngine.Object;

public class PreLevelCellModel : BaseModel<PreLevelCellView>
{
    public Transform CachedTransform { get; private set; }
    public int CellIndex { get; private set; }
    
    private PreLevelWeaponModel weaponModel;

    public PreLevelCellModel(PreLevelCellView view)
    {
        View = view;
        CellIndex = View.CellIndex;
        CachedTransform = View.transform;
        weaponModel = null;
    }

    public void SetWeaponModel(PreLevelWeaponModel model)
    {
        weaponModel = model;
    }

    public PreLevelWeaponModel GetWeaponModel()
    {
        return weaponModel;
    }
    
    public bool HasWeaponModel()
    {
        if (weaponModel != null)
        {
            return true;
        }

        return false;
    }

    public void ClearCurrentWeapon()
    {
        weaponModel = null;
    }

    public bool CompareMergeType(EWeaponsType mergeType)
    {
        var count = Enum.GetNames(typeof(EWeaponsType)).Length - 1;
        var lastMergeEnum = (EWeaponsType) count;
        if (weaponModel.MergeType == mergeType && mergeType < lastMergeEnum)
        {
            return true;
        }

        return false;
    }

    public void ClearData()
    {
        ClearCurrentWeapon();
        CachedTransform = null;
        
        Object.Destroy(View.gameObject);
    }
}