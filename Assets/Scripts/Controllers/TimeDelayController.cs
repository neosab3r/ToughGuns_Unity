using System;
using System.Collections.Generic;
using OLS_HyperCasual;

public class TimeDelayController : BaseController
{
    public override bool HasUpdate => true;
    private List<TimeDelayModel> modelsList = new List<TimeDelayModel>();

    public void StartDelay(float timeDelay, Action func)
    {
        TimeDelayModel timeDelayModel = new TimeDelayModel(timeDelay, func);
        modelsList.Add(timeDelayModel);
    }

    public override void Update(float dt)
    {
        for (int i = modelsList.Count - 1; i >= 0; i--)
        {
            var model = modelsList[i];
            
            model.UpdateTime(dt);
            
            if (model.isUse == false)
            {
                modelsList.Remove(model);
            }
        }
    }
}