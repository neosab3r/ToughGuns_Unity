using OLS_HyperCasual;
using UnityEngine;

public class CameraMoveController: BaseMonoController<CameraMoveView, CameraMoveModel>
{
    public override bool HasUpdate => true;

    private StartLevelWeaponController startLevelWeaponController;

    public override void PostInit()
    {
        startLevelWeaponController = BaseEntryPoint.Get<StartLevelWeaponController>();
    }

    public override CameraMoveModel AddView(CameraMoveView view)
    {
        var model = new CameraMoveModel(view);
        
        modelsList.Add(model);
        return model;
    }

    public override void Update(float dt)
    {
        foreach (var model in modelsList)
        {
            if (startLevelWeaponController.IsAnyWeaponNotScreen(model.MinBoundsPos, model.MaxBoundsPos, out Vector3 position))
            {
                model.SetCameraPosition(position);
            }
        }
    }
}