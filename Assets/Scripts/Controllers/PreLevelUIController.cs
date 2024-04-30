using OLS_HyperCasual;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreLevelUIController : BaseMonoController<PreLevelUIView, PreLevelUIModel>
{
    public override bool HasUpdate => false;
    
    private readonly string weaponInitialKey = "weaponInitial";

    private PlayerResourceModel softResourceModel;
    private StartLevelWeaponController startLevelWeaponController;
    private PreLevelWeaponController preLevelWeaponController;
    private PreLevelCellController cellController;
    private ZoneEnemyController zoneEnemyController;
    private SaveLevelController saveLevelController;
    private int weaponPrice;
    
    public override void PostInit()
    {
        softResourceModel = BaseEntryPoint.Get<PlayerResourcesController>().GetResourceModel(PResourceType.Soft);
        weaponPrice = ResourcesController.GetSettings().GetIntValue(weaponInitialKey, false);
        preLevelWeaponController = BaseEntryPoint.Get<PreLevelWeaponController>();
        cellController = BaseEntryPoint.Get<PreLevelCellController>();
        startLevelWeaponController = BaseEntryPoint.Get<StartLevelWeaponController>();
        zoneEnemyController = BaseEntryPoint.Get<ZoneEnemyController>();
        
        saveLevelController = BaseEntryPoint.Get<SaveLevelController>();

        zoneEnemyController.OnAllZonesDestroyed += OnLevelWin;
        cellController.OnAllCellsInited += LoadData;
        startLevelWeaponController.OnAllWeaponsDeath += OnLevelFailed;
    }

    private void LoadData()
    {
        var levelData = saveLevelController.GetLevelData(SceneManager.GetActiveScene().buildIndex);
        
        if (levelData != null)
        {
            preLevelWeaponController.CreateWeaponInData(levelData);
        }
    }

    public override PreLevelUIModel AddView(PreLevelUIView view)
    {
        var model = new PreLevelUIModel(view, OnClickButtonListener);
        modelsList.Add(model);

        return model;
    }
    
    public void OnClickButtonListener(EPreLevelButtonType type)
    {
        switch (type)
        {
            case EPreLevelButtonType.StartLevel:
            {
                StartLevel();
                break;
            }
            case EPreLevelButtonType.BuyWeapon:
            {
                BuyWeapon();
                break;
            }
            case EPreLevelButtonType.RestartLevel:
            {
                RestartLevel();
                break;
            }
        }
    }

    private void OnLevelWin()
    {
        saveLevelController.SaveLevelData(SceneManager.GetActiveScene().buildIndex,
            preLevelWeaponController.GetListModels(), startLevelWeaponController.GetDeathWeaponsIndex());
    }

    private void OnLevelFailed()
    {
        foreach (var model in modelsList)
        {
            if (model.View.Type == EPreLevelButtonType.RestartLevel)
            {
                model.SetTurnButton(true);
            }
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void StartLevel()
    {
        foreach (var model in modelsList)
        {
            model.SetTurnButton(false);
        }
        
        startLevelWeaponController.CreateModels(preLevelWeaponController.GetListModels());
        preLevelWeaponController.ClearData();
        cellController.ClearData();
    }

    private void BuyWeapon()
    {
        if (softResourceModel.HasEnoughResource(weaponPrice) && cellController.HasEmptyCell())
        {
            preLevelWeaponController.CreateWeapon(EWeaponsType.Pistol);
            softResourceModel.SpendResource(weaponPrice);
        }
    }
}