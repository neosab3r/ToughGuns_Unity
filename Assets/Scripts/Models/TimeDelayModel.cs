using System;
using UnityEngine;

public class TimeDelayModel
{
    private float delayTime;
    private Action onTimeDelaEnd;
    public bool isUse;

    public TimeDelayModel(float timeDelay, Action func)
    {
        delayTime = timeDelay;
        onTimeDelaEnd += func;
        isUse = true;
    }

    private void EndDelay()
    {
        onTimeDelaEnd?.Invoke();
        onTimeDelaEnd = null;
        isUse = false;
    }

    public void UpdateTime(float dt)
    {
        delayTime -= dt;
        if (delayTime <= 0)
        {
            EndDelay();
        }
    }
}