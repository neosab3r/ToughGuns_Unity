using System;
using OLS_HyperCasual;
using UnityEngine;

public class PreLevelUIModel : BaseModel<PreLevelUIView>
{
    private int weaponPrice;
    private PlayerResourceModel softResourceModel;

    public PreLevelUIModel(PreLevelUIView view, Action<EPreLevelButtonType> onClickBuy)
    {
        View = view;
        View.Button.onClick.AddListener(() =>
        {
            onClickBuy.Invoke(view.Type);
        });
        if (View.Type == EPreLevelButtonType.RestartLevel)
        {
            SetTurnButton(false);
        }
    }

    public void SetTurnButton(bool visible)
    {
        View.gameObject.SetActive(visible);
    }
}