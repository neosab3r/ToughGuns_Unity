using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using OLS_HyperCasual;
using UnityEngine;

public class BulletPoolableController : PoolableController<BulletModel, BulletView>
{
    public override bool HasUpdate => true;

    private EnemyController enemyController;
    private TimeDelayController timeDelayController;
    private StartLevelWeaponController startLevelWeaponController;
    private ZoneEnemyController zoneEnemyController;
    private GameEffectsController effectsController;
    private List<BulletModel> modelList = new();

    private const ESuffixGameType suffixGameType = ESuffixGameType.Bullet;
    private const float spawnBulletDelay = 0.1f;

    public BulletPoolableController()
    {
        timeDelayController = BaseEntryPoint.Get<TimeDelayController>();
        effectsController = BaseEntryPoint.Get<GameEffectsController>();
        var count = Enum.GetNames(typeof(EWeaponsType)).Length;
        var prefabsController = BaseEntryPoint.Get<PrefabsController>();

        var name = String.Concat(EWeaponsType.Pistol.ToString(), suffixGameType.ToString());
        var prefab = prefabsController.GetPrefab<BulletView>(EWeaponsType.Pistol, suffixGameType, false);
        PreInitPool(name, prefab);
    }

    public override void PostInit()
    {
        enemyController = BaseEntryPoint.Get<EnemyController>();
        startLevelWeaponController = BaseEntryPoint.Get<StartLevelWeaponController>();
        zoneEnemyController = BaseEntryPoint.Get<ZoneEnemyController>();
    }

    public void UseBullets(StartLevelWeaponModel weaponModel)
    {
        var key = GetKeyPooledBullet(weaponModel.WeaponType);
        var count = (int) weaponModel.RateFire + 1;

        for (int i = 0; i < count; i++)
        {
            timeDelayController.StartDelay(spawnBulletDelay * i, () => SpawnBulletWithDelay(key, weaponModel));
        }
    }

    private void SpawnBulletWithDelay(string key, StartLevelWeaponModel weaponModel)
    {
        effectsController.ShowEffect(weaponModel.EffectShootWeaponType.ToString(),
            weaponModel.CachedPointToShootTransform.position, weaponModel.CachedPointToShootTransform.rotation);
        
        var bullet = GetBullet(key);

        bullet.ShowBullet(weaponModel.WeaponDamage, weaponModel.CachedPointToShootTransform.position,
            weaponModel.CachedPointToShootTransform.rotation);
    }

    private BulletModel GetBullet(string key)
    {
        var newModel = true;
        var bullet = GetFromPool(key);

        foreach (var model in modelList)
        {
            if (model == bullet)
            {
                newModel = false;
            }
        }

        if (newModel)
        {
            modelList.Add(bullet);
        }

        return bullet;
    }

    private void ReturnBullet(BulletModel model)
    {
        ReturnToPool(model);
        model.HideBullet();
    }

    private string GetKeyPooledBullet(EWeaponsType weaponType)
    {
        var key = String.Concat(EWeaponsType.Pistol.ToString(), suffixGameType.ToString());

        return key;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Update(float dt)
    {
        for (int i = modelList.Count - 1; i >= 0; i--)
        {
            var model = modelList[i];

            if (model.IsUse == false)
            {
                continue;
            }

            model.UpdatePosition(dt);

            if (model.LifeTime <= 0)
            {
                ReturnBullet(model);
            }
            else if (zoneEnemyController.IsAnyZoneModelContainsObject(model.CachedTransform.position))
            {
                ReturnBullet(model);
            }
            else if (enemyController.IsAnyEnemyContainsPosition(model.CachedTransform.position, model.WeaponDamage))
            {
                startLevelWeaponController.OnEnemyReceiveDamage();
                ReturnBullet(model);
            }
        }
    }
}