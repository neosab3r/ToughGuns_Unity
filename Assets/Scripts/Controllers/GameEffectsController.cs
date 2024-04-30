using OLS_HyperCasual;
using UnityEngine;

public class GameEffectsController:  PoolableController<EffectsModel, EffectsView>
{
    public override bool HasUpdate => true;
        
    public void ShowEffect(string effectType, Vector3 position, Quaternion rotation)
    {
        var effect = GetFromPool(effectType);
        effect.View.transform.rotation = rotation;
        effect.ShowEffect(position);
    }
        
    public override void Update(float dt)
    {
        foreach (var kv in pooledModelsDict)
        {
            foreach (var kvModels in kv.Value)
            {
                var model = kvModels.Value;
                if (model.IsInPool)
                {
                    continue;
                }
                    
                if (model.IsAlive())
                {
                    model.UpdateLifeTime(dt);
                    continue;
                }

                ReturnToPool(model);
            }
        }
    }
}