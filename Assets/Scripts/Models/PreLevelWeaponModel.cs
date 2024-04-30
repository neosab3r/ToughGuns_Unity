using com.ols.merge2.Game.Models;
using DG.Tweening;
using OLS_HyperCasual;
using UnityEngine;

public class PreLevelWeaponModel : Merge2MonoModel<PreLevelWeaponView, PreLevelWeaponModel>
{
    public EWeaponsType MergeType;
    private Transform cachedTransform;
    private PreLevelCellModel currentCellModel;

    public PreLevelWeaponModel(PreLevelWeaponView view, GetMonoMerge getMonoMerge) : base(getMonoMerge)
    {
        View = view;
        cachedTransform = View.transform;
    }

    public void OnBeginDrag(Vector3 position)
    {
    }

    public void OnDrag(Vector3 mousePosition)
    {
        SetViewPosition(mousePosition);
    }

    public void EndDrag(PreLevelCellModel cell)
    {
        if (cell != null && cell.HasWeaponModel() == false)
        {
            currentCellModel.ClearCurrentWeapon();

            cell.SetWeaponModel(this);

            currentCellModel = cell;
        }

        SetViewPosition(currentCellModel.CachedTransform.position);
    }

    public PreLevelCellModel GetCachedCell()
    {
        return currentCellModel;
    }

    public void ClearData()
    {
        Object.Destroy(View.gameObject);

        currentCellModel = null;
        cachedTransform = null;
        View = null;
    }
    
    public void SetCellModel(PreLevelCellModel cellModel)
    {
        currentCellModel = cellModel;
        currentCellModel.SetWeaponModel(this);
        SetViewPosition(currentCellModel.CachedTransform.position);
    }

    private void SetViewPosition(Vector3 position)
    {
        cachedTransform.position = position;
    }
}