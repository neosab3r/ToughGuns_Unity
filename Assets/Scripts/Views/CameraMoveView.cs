using System;
using OLS_HyperCasual;
using UnityEngine;

public class CameraMoveView : MonoBehaviour
{
    [field: SerializeField] public Camera Camera { get; private set; }
    [field: SerializeField] public Collider Collider { get; private set; }

    private void Start()
    {
        var entry = BaseEntryPoint.GetInstance();
        entry.SubscribeOnBaseControllersInit(() =>
        {
            entry.GetController<CameraMoveController>().AddView(this);
        });
    }
}