using System.Collections.Generic;
using OLS_HyperCasual;
using UnityEngine;

public class SortCellByIndex : MonoBehaviour
{
    [field: SerializeField] private List<PreLevelCellView> listView;

    private void Start()
    {
        var entry = BaseEntryPoint.GetInstance();
        entry.SubscribeOnBaseControllersInit(() =>
        {
            entry.GetController<PreLevelCellController>().SetCountCell(listView.Count);
        });
        
        foreach (var cellView in listView)
        {
            entry.SubscribeOnBaseControllersInit(() =>
            {
                entry.GetController<PreLevelCellController>().AddView(cellView);
            });
        }
        
        listView.Clear();
    }
}