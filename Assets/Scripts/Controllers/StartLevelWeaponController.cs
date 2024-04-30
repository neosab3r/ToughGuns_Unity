using System;
using System.Collections;
using System.Collections.Generic;
using OLS_HyperCasual;
using UnityEngine;

public class StartLevelWeaponController : BaseMonoController<StartLevelWeaponView, StartLevelWeaponModel>
{
    public override bool HasUpdate => true;

    public Action OnAllWeaponsDeath;
    private Action onUpdateModelsPosition;
    private Action onAllWeaponsLoad;

    private PrefabsController prefabsController;
    private CoroutineController coroutineController;
    private StartLevelWeaponView pistolPrefab;
    private StartLevelWeaponView riflePrefab;
    private StartLevelWeaponView sniperRiflePrefab;
    private BulletPoolableController bulletPoolableController;
    private GameEffectsController effectsController;
    private EnemyController enemyController;
    private ZoneEnemyController zoneEnemyController;

    private BorderModel borderModel;
    private ParentWeaponModel parentWeaponModel;

    private bool isButtonDelay = true;
    private float currentFireDelayTime;

    private const ESuffixGameType suffixGameType = ESuffixGameType.RB;
    private const float DefaultFireDelayTime = 0.4f;
    private const float DefaultTimeScaleValue = 1f;
    private const float SlowdownTimeScaleValue = 0.3f;

    public StartLevelWeaponController()
    {
        prefabsController = BaseEntryPoint.Get<PrefabsController>();
        effectsController = BaseEntryPoint.Get<GameEffectsController>();
        coroutineController = BaseEntryPoint.Get<CoroutineController>();
        bulletPoolableController = BaseEntryPoint.Get<BulletPoolableController>();
        
        var count = Enum.GetNames(typeof(EEffectShootWeaponType)).Length;
        for (int i = 0; i < count; i++)
        {
            var prefabEffect = prefabsController.GetPrefab<EffectsView>((EEffectShootWeaponType) i, null, false);
            effectsController.PreInitPool(((EEffectShootWeaponType)i).ToString(),  prefabEffect);

        }
    }

    public override void PostInit()
    {
        enemyController = BaseEntryPoint.Get<EnemyController>();
        zoneEnemyController = BaseEntryPoint.Get<ZoneEnemyController>();
    }

    public override StartLevelWeaponModel AddView(StartLevelWeaponView view)
    {
        var model = new StartLevelWeaponModel(view);

        onAllWeaponsLoad += model.SetKinematic;
        onUpdateModelsPosition += model.UpdatePosition;

        modelsList.Add(model);

        return model;
    }

    public ParentWeaponModel AddParentView(ParentWeaponView view)
    {
        parentWeaponModel = new ParentWeaponModel(view);
        return parentWeaponModel;
    }

    public void AddBorderView(BorderView borderView)
    {
        borderModel = new BorderModel(borderView);
    }

    public void CreateModels(List<PreLevelWeaponModel> preLevelModels)
    {
        foreach (var preLevelModel in preLevelModels)
        {
            EWeaponsType weaponType = preLevelModel.MergeType;
            var cell = preLevelModel.GetCachedCell();
            var view = CreateView(cell.CachedTransform.position, weaponType);

            var model = AddView(view);
            model.CellIndex = cell.CellIndex;
            model.WeaponType = weaponType;
        }

        onAllWeaponsLoad?.Invoke();

        isButtonDelay = false;
    }
    
    public void OnEnemyReceiveDamage()
    {
        //enemyController.ReduceHP(model, "", damage);
        coroutineController.StartCoroutine(TimeScaleCoroutine());
    }
    
    public bool IsAnyWeaponNotScreen(Vector3 modelMinBoundsPos, Vector3 modelMaxBoundsPos, out Vector3 positionToCamera)
    {
        foreach (var model in modelsList)
        {
            if (model.isUse == false)
            {
                continue;
            }
            
            if (model.CachedTransform.position.y <= modelMinBoundsPos.y ||
                model.CachedTransform.position.y >= modelMaxBoundsPos.y)
            {
                positionToCamera = model.CachedTransform.position;
                return true;
            }
        }
        positionToCamera = Vector3.zero;
        return false;
    }

    public List<int> GetDeathWeaponsIndex()
    {
        List<int> list = new List<int>();

        foreach (var model in modelsList)
        {
            if (model.isUse == false)
            {
                list.Add(model.CellIndex);
            }
        }

        return list;
    }

    private StartLevelWeaponView CreateView(Vector3 position, EWeaponsType weaponType)
    {
        var gameObject = prefabsController.GetPrefab<StartLevelWeaponView>(weaponType, suffixGameType);

        gameObject.transform.SetParent(parentWeaponModel.View.transform);
        gameObject.transform.position = position;

        return gameObject;
    }

    private void UpdateWeaponsZonePosition()
    {
        foreach (var model in modelsList)
        {
            if (borderModel.IsInsideZone(model.CachedTransform.position) == false)
            {
                var pos = borderModel.GetBoundPosition(model.CachedTransform.position, out var isRight);

                model.SetBorderPosition(pos, isRight);
            }

            if (zoneEnemyController.IsAnyZoneModelContainsObject(model.CachedTransform.position))
            {
                RemoveWeapon(model);
            }
            if (enemyController.IsAnyEnemyContainsPosition(model.CachedTransform.position))
            {
                RemoveWeapon(model);
            }
        }
    }

    private void RemoveWeapon(StartLevelWeaponModel weaponModel)
    {
        if (weaponModel.isUse)
        {
            weaponModel.DeathWeapon();
        }

        foreach (var model in modelsList)
        {
            if (model.isUse)
            {
                return;
            }
        }
        
        OnAllWeaponsDeath?.Invoke();
        OnAllWeaponsDeath = null;
    }

    private IEnumerator TimeScaleCoroutine()
    {
        Time.timeScale = SlowdownTimeScaleValue;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = DefaultTimeScaleValue;
    }

    private void SetButtonState(bool state)
    {
        isButtonDelay = state;
        currentFireDelayTime = DefaultFireDelayTime;
    }

    private void ReduceFireDelayTime(float dt)
    {
        if (isButtonDelay == false)
        {
            return;
        }

        currentFireDelayTime -= dt;
        if (currentFireDelayTime <= 0)
        {
            SetButtonState(false);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void Update(float dt)
    {
        if (Input.GetMouseButtonDown(0) && isButtonDelay == false)
        {
            SetButtonState(true);

            onUpdateModelsPosition?.Invoke();

            foreach (var model in modelsList)
            {
                if (model.isUse == false)
                {
                    continue;
                }
                
                bulletPoolableController.UseBullets(model);

                model.Shoot();
            }
        }

        ReduceFireDelayTime(dt);

        UpdateWeaponsZonePosition();
    }
}