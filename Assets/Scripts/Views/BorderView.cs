using OLS_HyperCasual;
using UnityEngine;

public class BorderView : MonoBehaviour
{
    [field: SerializeField] public Collider Collider { get; private set; }
    private void Start()
    {
        var entry = BaseEntryPoint.GetInstance();
        entry.SubscribeOnBaseControllersInit(() =>
        {
            entry.GetController<StartLevelWeaponController>().AddBorderView(this);
        });
    }
}