using OLS_HyperCasual;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PreLevelUIView : MonoBehaviour
{
    [field: SerializeField] public EPreLevelButtonType Type;
    [field: SerializeField] public Button Button;

    private void Start()
    {
        var entry = BaseEntryPoint.GetInstance();
        entry.SubscribeOnBaseControllersInit(() =>
        {
            entry.GetController<PreLevelUIController>().AddView(this);
        });
    }
}