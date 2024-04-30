using System;
using OLS_HyperCasual;
using RayFire;
using UnityEngine;

public class ZoneEnemyModel : BaseModel<ZoneEnemyView>
{
    public int CountEnemy { get; private set; }
    public RayfireRigid rayFireRigid { get; private set; }
    private Bounds bounds;

    private Action<ZoneEnemyModel> onDestroyAction;

    public ZoneEnemyModel(ZoneEnemyView view, Action<ZoneEnemyModel> onDeleteModelInList)
    {
        View = view;
        bounds = View.Collider.bounds;

        rayFireRigid = view.RayfireRigid;

        onDestroyAction += onDeleteModelInList;

        CountEnemy = view.enemyViews.Count;
    }

    public void ReduceCountEnemy()
    {
        CountEnemy -= 1;

        if (CountEnemy <= 0)
        {
            rayFireRigid.Demolish();
            onDestroyAction?.Invoke(this);
        }
    }

    public bool IsContains(Vector3 position)
    {
        if (bounds.Contains(position))
        {
            return true;
        }

        return false;
    }
}