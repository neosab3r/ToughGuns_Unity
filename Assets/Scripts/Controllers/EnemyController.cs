using System.Collections;
using OLS_HyperCasual;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : BaseMonoController<EnemyView, EnemyModel>
{
    public override bool HasUpdate => true;

    private HPBarSnake hpSlider;
    private CoroutineController coroutineController;
    private ZoneEnemyController zoneEnemyController;
    private TimeDelayController timeDelayController;
    private GameEffectsController effectsController;

    private const float VisualizeDamageDelay = 0.3f;
    private const float TimeToDestroyView = 1f;

    public EnemyController()
    {
        zoneEnemyController = BaseEntryPoint.Get<ZoneEnemyController>();
        coroutineController = BaseEntryPoint.Get<CoroutineController>();
        timeDelayController = BaseEntryPoint.Get<TimeDelayController>();
        effectsController = BaseEntryPoint.Get<GameEffectsController>();

        var prefabsController = BaseEntryPoint.Get<PrefabsController>();
        var prefabEffectBody =
            prefabsController.GetPrefab<EffectsView>(EEffectDamageEnemyType.BodyShotEffect, null, false);
        var prefabEffectHead =
            prefabsController.GetPrefab<EffectsView>(EEffectDamageEnemyType.HeadShotEffect, null, false);

        effectsController.PreInitPool(EEffectDamageEnemyType.BodyShotEffect.ToString(), prefabEffectBody);
        effectsController.PreInitPool(EEffectDamageEnemyType.HeadShotEffect.ToString(), prefabEffectHead);
    }

    public override EnemyModel AddView(EnemyView view)
    {
        var model = new EnemyModel(view);

        modelsList.Add(model);

        return model;
    }

    public void AddSnakeHPBar(HPBarSnake slider)
    {
        hpSlider = slider;
    }

    public bool IsAnyEnemyContainsPosition(Vector3 position, int weaponDamage)
    {
        foreach (var model in modelsList)
        {
            if (model.IsDeath)
            {
                continue;
            }

            if (model.IsContains(position, out string nameBody))
            {
                if (nameBody == "mixamorig:Spine1")
                {
                    effectsController.ShowEffect(EEffectDamageEnemyType.BodyShotEffect.ToString(), position,
                        new Quaternion());
                }
                else if (nameBody == "mixamorig:Head")
                {
                    effectsController.ShowEffect(EEffectDamageEnemyType.HeadShotEffect.ToString(), position,
                        new Quaternion());
                }

                //model.ReduceHP(nameBody, weaponDamage);
                hpSlider.Damage(weaponDamage);
                if (model.IsDeath == false)
                {
                    coroutineController.StartCoroutine(VisualizeDamageCoroutine(model));
                }

                return true;
            }
        }

        return false;
    }

    public bool IsAnyEnemyContainsPosition(Vector3 position)
    {
        foreach (var model in modelsList)
        {
            if (model.IsDeath)
            {
                continue;
            }

            if (model.IsContains(position))
            {
                return true;
            }
        }

        return false;
    }

    private void RemoveModel(EnemyModel model)
    {
        if (model.View != null)
        {
            Object.Destroy(model.View.gameObject);
        }

        modelsList.Remove(model);
    }

    private IEnumerator VisualizeDamageCoroutine(EnemyModel model)
    {
        model.View.VisualizeDamage(true);
        yield return new WaitForSeconds(VisualizeDamageDelay);
        model.View.VisualizeDamage(false);
    }

    public override void Update(float dt)
    {
        Debug.Log(modelsList.Count);
        for (int i = modelsList.Count - 1; i >= 0; i--)
        {
            var model = modelsList[i];

            if (model.IsDeath)
            {
                model.ClearData();
                timeDelayController.StartDelay(TimeToDestroyView, () => RemoveModel(model));
            }
        }
    }
}