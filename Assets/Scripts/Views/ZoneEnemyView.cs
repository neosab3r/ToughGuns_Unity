using System.Collections.Generic;
using OLS_HyperCasual;
using RayFire;
using UnityEngine;

public class ZoneEnemyView : MonoBehaviour
{
    [field: SerializeField] public List<EnemyView> enemyViews { get; private set; }
    [field: SerializeField] public RayfireRigid RayfireRigid { get; private set; }
    [field: SerializeField] public Collider Collider { get; private set; }
    private void Start()
    {
        var entry = BaseEntryPoint.GetInstance();
        entry.SubscribeOnBaseControllersInit(() =>
        {
            entry.GetController<ZoneEnemyController>().AddView(this);
        });
    }
}