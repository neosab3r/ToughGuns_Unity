using OLS_HyperCasual;
using UnityEngine;

public class ParentWeaponView : MonoBehaviour
{
    private void Start()
    {
        var entry = BaseEntryPoint.GetInstance();
        entry.SubscribeOnBaseControllersInit(() =>
        {
            entry.GetController<StartLevelWeaponController>().AddParentView(this);
        });
    }
}